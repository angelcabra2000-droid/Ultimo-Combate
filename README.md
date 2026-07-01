# 🥊 Último Combate

Proyecto final de Animación 3D y Dinámicas — Ingeniería Multimedia, Universidad Militar Nueva Granada.

**Último Combate** es una experiencia interactiva en la que el jugador controla a un boxeador dentro de un gimnasio de entrenamiento. El objetivo es practicar distintas actividades —como golpear sacos de boxeo y levantar pesas— para ganar experiencia, mientras administra su energía recuperándola en las estaciones de hidratación distribuidas por el escenario.


> Este repositorio documenta principalmente el desarrollo en **Unity/C#**: la lógica de juego, la física de los sacos de boxeo, el sistema de energía y la interacción del jugador con los objetos del gimnasio.

## 🎮 Controles

| Acción | Tecla |
|---|---|
| Movimiento | `WASD` |
| Cámara | Mouse |
| Guardia | `G` |
| Jab | `Q` |
| Recto | `E` |
| Hook | `X` |
| Uppercut | `C` |
| Tomar agua | `F` |
| Levantar pesas | `R` |
| Esquivo 1 | `Shift + Q` |
| Esquivo 2 | `Shift + E` |
| Esquivo 3 | `Shift + X` |
| Esquivo 4 | `Shift + C` |

## ⚡ Sistema de energía

| Recuperan energía | Consumen energía |
|---|---|
| Consumir agua | Golpear sacos |
| | Levantar pesas |

- **Energía**: representa la resistencia física del boxeador.
- **Experiencia**: representa el progreso obtenido durante el entrenamiento.

## 🕺 Animaciones implementadas

- **Locomoción**: caminar, guardia
- **Combate**: jab, cross, hook, uppercut
- **Interacción**: tomar agua, levantar pesas

## 🧩 Objetos interactivos

| Objeto | Función |
|---|---|
| Saco de boxeo | Entrenamiento de golpes |
| Pesas | Incremento de experiencia |
| Botellas de agua | Recuperación de energía |

## 🛠️ Tecnologías

- **Motor**: Unity 6000.0.75f1
- **Lenguaje**: C#
- **Modelado y animación**: Blender
- **Sistema de input**: Unity Input System

## 🔭 Visión del proyecto

Esta versión representa una fase previa a un futuro modo carrera de boxeo, enfocada en la preparación física del atleta. Sienta las bases para futuras expansiones que incluirán combates, torneos y el desarrollo profesional del personaje:

```
Entrenamiento (actual) → Combates (futuro) → Torneos
```

## 📸 Gameplay

<!-- Agrega aquí tus capturas o GIF del gameplay -->

## 🚀 Cómo abrir el proyecto

1. Clona este repositorio:
   ```bash
   git clone https://github.com/angelcabra2000-droid/Ultimo-Combate.git
   ```
2. Abre el proyecto con **Unity Hub**, usando la versión **6000.0.75f1**.
3. Abre la escena `Assets/Scenes/SampleScene.unity`.
4. Dale play ▶️ en el editor.
