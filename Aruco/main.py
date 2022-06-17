#!/usr/bin/env python
import asyncio
import threading
import PySimpleGUI as sg
import cv2
import numpy as np
import mouse
import pyautogui
import CalibrationGUI
import UnityConnect as UConn
from opencvAruco import detectMarkers

"""
ARUCO CONDIFUGRATIONS
"""
matrix = np.loadtxt('CamSettings/matrix.txt')
dist = np.loadtxt('CamSettings/dist.txt')

# DETECTOR ARUCO
# Inicializamos los parámetros del detector de arucos
parametros = cv2.aruco.DetectorParameters_create()

# Cargamos el diccionario de nuestro Aruco
diccionario = cv2.aruco.Dictionary_get(cv2.aruco.DICT_5X5_100)


"""
GUI IMAGES
"""
hello_run_img = cv2.imread('imgs/GUIs/hello_run.PNG')
hello_run_img = cv2.resize(hello_run_img, (512, 384), interpolation=cv2.INTER_AREA)

no_recording_img = cv2.imread("imgs/GUIs/no_rec.png")
no_recording_img = cv2.resize(no_recording_img, (640, 480), interpolation=cv2.INTER_AREA)


"""
COLUMN LAYERS
"""
col1 = [
    [sg.Text('Instrucciones', size=(15,1), justification='left', font='Helvetica 10')],
    [sg.Button('Calibrador', size=(10, 1), font='Helvetica 14')],
    [sg.Button('Aruco Defensor', size=(15, 1), font='Helvetica 14')],
    [sg.Button('Aruco Atacante', size=(15, 1), font='Helvetica 14')],
    [sg.Button('Salir', size=(10, 1), font='Helvetica 14')],
]

col2 = [
    [sg.Multiline('Pasos básicos a seguir, clic en los diferentes botones: \n'
             '1) Calibrar la cámara [botón: Calibrador]\n'
             '2) Detección de marcadores Aruco [botón: Aruco]\n',
             size=(50, 4), justification='left', font='Helvetica 10', key='simple_instruction')],
    [sg.Image(key='image')],
    [
        sg.Button('Grabar', size=(10, 1), font='Helvetica 14'),
        sg.Button('Detener', size=(10, 1), font='Any 14'),
    ],
    [sg.Text('', size=(40, 1), justification='left', font='Helvetica 10', key='Logs')],
]


def main():
    sg.theme('DarkTeal9')

    layout = [
        [
            sg.Column(col1, element_justification='c'),
            sg.Column(col2, element_justification='c'),
        ]
    ]

    # create the window and show it without the plot
    window = sg.Window('Demo marcadores Aruco',
                       layout, location=(500, 400))

    # ---===--- Event LOOP Read and display frames, operate the GUI --- #
    cap = cv2.VideoCapture(0)

    aruco_mode = False
    recording_cam = False


    scr_width, scr_height = pyautogui.size()
    first_time = False

    para_atacante = False
    para_defensor = False

    # Antes nos conectamos al servidor
    socket_defensor = UConn.connect_to_server("127.0.0.1", 25001) # puerto para el defensor
    socket_atacante = UConn.connect_to_server("127.0.0.1", 25005) # puerto para el atacante

    while True:
        event, values = window.read(timeout=10)
        if event == 'Salir' or event == sg.WIN_CLOSED:
            break

        elif event == "Calibrador":
            aruco_mode = False
            recording_cam = False
            para_atacante = para_defensor = False
            CalibrationGUI.calibration_window()

        elif event == "Aruco Defensor":
            aruco_mode = True
            recording_cam = True

            para_defensor = True
            para_atacante = False
            window['simple_instruction'].update("Muestra un marcador Aruco de tamaño 5x5.\n"
                                                "Ya puedes usar el marcador para jugar como defensa")
        elif event == "Aruco Atacante":
            aruco_mode = True
            recording_cam = True

            para_atacante = True
            para_defensor = False
            window['simple_instruction'].update("Muestra un marcador Aruco de tamaño 5x5.\n"
                                                "Ya puedes usar el marcador para jugar como defensa")

        elif event == 'Grabar':
            if aruco_mode:
                recording_cam = True

        elif event == 'Detener':
            if aruco_mode:
                recording_cam = False
                imgbytes = cv2.imencode('.png', no_recording_img)[1].tobytes()
                window['image'].update(data=imgbytes)

        if recording_cam:
            ret, frame = cap.read()

            frame_width = frame.shape[1]
            frame_height = frame.shape[0]

            aruco_detected, corners = detectMarkers.paintMarker(frame, diccionario, parametros, matrix, dist)

            if len(corners) != 0:
                centers_aruco = []
                for cor in corners:
                    center_ar = list(np.sum(cor[0], axis=0, dtype=np.int) // 4)
                    center_ar.append(0)
                    # enviamos el centro de nuestro marcador aruco a Unity
                    centers_aruco.append(center_ar)
                if para_defensor:
                    # enviamos el caracter del defensor
                    UConn.send_positions(socket_defensor, centers_aruco, 'd', False)
                elif para_atacante: # sino la única pposibilidad es para el atacante
                    UConn.send_positions(socket_atacante, centers_aruco, 'a', False)

            img_bytes = cv2.imencode('.png', aruco_detected)[1].tobytes()
            window['image'].update(data=img_bytes)

    window.close()
    cap.release()

main()

