#!/usr/bin/env python
import asyncio
import threading
import PySimpleGUI as sg
import pyautogui
import CalibrationGUI
import UnityConnect as UConn


"""
COLUMN LAYERS
"""
col1 = [
    [sg.Text('Atacante', size=(15, 1), justification='left', font='Helvetica 13')],
    [sg.Text('Disparos: 0', size=(15, 1), justification='left', font='Helvetica 10', key='-DISPAROS_ATACANTE-')],
    [sg.Text('Puntaje: 0', size=(15, 1), justification='left', font='Helvetica 10', key='-PUNTAJE_ATACANTE-')],

    [sg.Text('Aves', size=(15, 1), justification='left', font='Helvetica 13')],
    [sg.Text('Aves cazadas: 0', size=(15, 1), justification='left', font='Helvetica 10', key='-AVES_MUERTAS-')],
]

col2 = [
    [sg.Text('Defensor', size=(15, 1), justification='left', font='Helvetica 13')],
    [sg.Text('Barreras: 0', size=(15, 1), justification='left', font='Helvetica 10', key='-BARRERAS_DEFENSOR-')],
    [sg.Text('Barreras destruidas: 0', size=(20, 1), justification='left', font='Helvetica 10',
             key='-BARRERAS_DESTRUIDAS_DEFENSOR-')],

    [sg.Text('Ganador', size=(15, 1), justification='left', font='Helvetica 13')],
    [sg.Text('Por definirse...', size=(15, 1), justification='left', font='Helvetica 10', key='-GANADOR-')],
]


def main():
    sg.theme('DarkTeal9')

    layout = [
        [sg.Text('Control de sistema', size=(40, 1), justification='center', font='Helvetica 10')],
        [
            sg.Column(col1, element_justification='c'),
            sg.Column(col2, element_justification='c'),
        ],
        [sg.Button('Salir', size=(10, 1), font='Helvetica 14')],
    ]

    # create the window and show it without the plot
    window = sg.Window('Control de sistema',
                       layout, location=(500, 400))


    # el puerto para el controlador del sistema es 25010
    socket_controlador = UConn.connect_to_server("127.0.0.1", 25010)

    # esta función demora mucho en ejecutarse
    def receiveControllerData():
        # recibiendo los datos del servidor con las estadísticas del juego
        statisticsGame = socket_controlador.recv(1024).decode("UTF-8")
        # separamos las estadísticas en comas
        statisticsGame = statisticsGame.split(",")
        return statisticsGame

    while True:
        event, values = window.read(timeout=10)
        if event == 'Salir' or event == sg.WIN_CLOSED:
            break

        # PySimpleGUI realizará la tarea multithreading por nosotros...
        window.perform_long_operation(receiveControllerData, '-CONTROLLER_OPERATION_DONE-')
        if event == "-CONTROLLER_OPERATION_DONE-":
            # values[event] es la variable statisticsGame retornada
            window['-DISPAROS_ATACANTE-'].update(f"Disparos: {values[event][0]}")
            window['-PUNTAJE_ATACANTE-'].update(f"Puntaje: {values[event][1]}")
            window['-AVES_MUERTAS-'].update(f"Aves cazadas: {values[event][0]}")
            window['-BARRERAS_DEFENSOR-'].update(f"Barreras: {values[event][2]}")
            window['-BARRERAS_DESTRUIDAS_DEFENSOR-'].update(f"Barreras destruidas: {values[event][3]}")
            print("Statistics game: ", values[event])


    window.close()

main()

