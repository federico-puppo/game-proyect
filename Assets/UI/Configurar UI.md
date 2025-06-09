Configuración de la escena UI

La escena UI (Mental_UI_Scene) se integra con el InventoryManager del proyecto principal. 

A continuación se detallan los pasos de configuración manual necesarios en Unity:


Etiquetas personalizadas (Tags)
En Unity, abra el gestor de etiquetas: en el Inspector del GameObject seleccione el desplegable Tag y elija Add Tag...
docs.unity3d.com. 
En la ventana Tags and Layers que aparece, cree:
__Slot: tag requerido por los scripts del inventario.
__Interactables (opcional): para agrupar/identificar objetos interactivos a nivel visual o semántico (no es obligatorio para los scripts).

Capa UI
Abra Edit > Project Settings > Tags and Layers > Layers
y agregue una nueva capa llamada UI. Esta capa se utiliza para identificar y gestionar los elementos de interfaz en la escena Mental_UI_Scene.
