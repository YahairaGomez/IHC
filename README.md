# Arrow and bow
El juego contará con dos personajes principales: El arquero y el ave, el objetivo el hacer que que el arquero apunte al ave con la flecha a fin de sumar puntos: 
Cada vez que la flecha choque con el ave se irá incrementando la cantidad de puntos. 
Para mover la flecha que apunta al ave se hará uso de reconocimiento de gestos y para disparar las flechas usaremos comandos de voz con palabras específicas.

### Requisitos 🔧

El usuario debe contar con una cámara web con buena resolución y un micrófono.
La mano debe ser colocada a una distancia de 1 metro como máximo y el micrófono también, el ambiente en donde se encuentra debe tener buena iluminación y estar libre de eco
o ruido que pueda interferir con el reconocimiento de comandos de voz.

El programa cuenta con una ventana de verificación de dispositivos conectados, si el usuario no cumple con los requisitos solicitados no podrá jugar el juego.
            

## Instrucciones ⚙️
Para mover la dirección del arco levante el dedo índice y desplácelo de arriba hacia abajo o en la dirección que usted desee. Cuando ya tenga lista su dirección de disparo
diga claramente "Dispara" o "Fuego" o "Flecha"

## Configuración ⚙️
Para instalar y correr el proyecto descargue la carpeta la carpeta completa y ejecute el programa.

### Demo 🔧

<img src="https://github.com/YahairaGomez/IA/blob/main/Images/input.png">

En este caso nosotros el valor del eje x será igual a 300, mientras que el eje y será igual a 500. Una vez ingresado el tamaño de la grilla el programa mostrará la grilla dibujada y lista para utilizar.

<img src="https://github.com/YahairaGomez/IA/blob/main/Images/grilla.png">
A continuación, procederemos a darle un punto de inicio y de llegada haciendo click izquierdo sobre los puntos que queramos (punto de inicio y de llegada)

<img src="https://github.com/YahairaGomez/IA/blob/main/Images/ini.png">

Luego, podemos borrar los puntos; es decir, bloquear caminos haciendo click derecho sobre los puntos que deseemos eliminar (sin  soltar el mouse)

<img src="https://github.com/YahairaGomez/IA/blob/main/Images/borrado.png">

Después presionamos 1 si queremos una búsqueda haciendo uso de Hill climbing o 2 si queremos usar A*, tal y como se ve en las imagenes (respectivamente).


<img src="https://github.com/YahairaGomez/IA/blob/main/Images/hillclimbing.png">
<img src="https://github.com/YahairaGomez/IA/blob/main/Images/a_star.png">
Si queremos hacer una nueva búsqueda simplemente seleccionamos los nuevos puntos y listo. En caso quiera borrar nuevos caminos o validar los que antes estaban bloqueados puede hacer click en la tecla 3, esto hará que nuestro grafo se resetee.
## Autor ✒️

* **Yahaira Gomez Sucasaca** - *Documentación* - [YahairaGomez](https://github.com/YahairaGomez)

## Expresiones de Gratitud 🎁

* Agradecimiento especial a YouTube, computación gráfica y a la cafeína ☕
