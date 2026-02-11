# Roadmap Técnico: "Void" (Working Title)

Este documento detalla el proceso de desarrollo integral de **Void**, un plataformas 2D desarrollado en Unity 6+.

---

## 📅 Resumen de Fases
1. **Fase 1: Cimientos y Arquitectura** (Pre-producción)
2. **Fase 2: Prototipo Dorado (Core Mechanics)**
3. **Fase 3: Sistemas de Juego y Progresión**
4. **Fase 4: Estética, VFX y Atmósfera**
5. **Fase 5: Producción de Contenido (Actos 1-3)**
6. **Fase 6: Pulido, UI y Audio Adaptativo**
7. **Fase 7: Optimización Multiplataforma (PC/Mobile)**
8. **Fase 8: QA, Gold Master y Lanzamiento**

---

## 🛠 Fase 1: Cimientos y Arquitectura
*Objetivo: Configurar el entorno de desarrollo y la base técnica.*

- [x] **Estructura de Carpetas:** Implementar convención `_Interlife/` (Assets, Scripts, Prefabs, Art, Audio, Shaders).
- [x] **Control de Versiones:** Git configurado con `.gitignore` para Unity y LFS para assets pesados.
- [x] **Input System:** Configuración de `InputSystem_Actions` para soporte híbrido (Teclado/Mando y Touch).
- [x] **Arquitectura de Código:** Definir `Manager` central (GameManager, LevelManager) usando el patrón Singleton o ScriptableObjects para persistencia.
- [x] **Herramientas de Editor:** Creado "Robot" de creación automática de Player (`PlayerCreator`).

---

## 🏃 Fase 2: Prototipo Dorado (Core Mechanics)
*Objetivo: Lograr el "Game Feel" de Void (Ligereza controlada).*

- [/] **Player Controller (2D):** 
    - [/] Movimiento horizontal constante (6 m/s) sin aceleración brusca.
    - [/] Salto Variable con Gravedad Asimétrica (subida suave, caída pesada).
    - [ ] Coyote Time y Jump Buffering para precisión máxima.
- [/] **Movimientos Especiales:**
    - [/] Dash Sombrío (Recorrido de 4 unidades en 0.2s).
    - [ ] Wall Jump (Rebote diagonal).
    - [ ] Ledge Grab (Detección de esquinas y animación de izado).
- [ ] **Cámara (Cinemachine):**
    - [ ] Configuración de Dead Zone y Soft Zones.
    - [ ] Implementación de "Look Ahead" (desplazamiento predictivo).

---

## ✨ Fase 3: Sistemas de Juego y Progresión
*Objetivo: Implementar el bucle principal (Core Loop).*

- [ ] **Sistema de Vida y Muerte:**
    - [ ] Detección de colisiones letales (Triggers vs Collisioners).
    - [ ] Sistema de Checkpoints (Guardado de posición en memoria).
    - [ ] Respawn instantáneo con efecto visual de disolución.
- [ ] **Coleccionables (Fragmentos):**
    - [ ] Logica de 3 fragmentos por nivel usando ScriptableObjects para trackeo.
- [ ] **IA Simple:**
    - [ ] Patrullas A-B (Movimiento rítmico).
    - [ ] Cazadores (Cono de visión y Dash hacia jugador).

---

## 🎨 Fase 4: Estética, VFX y Atmósfera
*Objetivo: Traducir el GDD visual a Unity.*

- [ ] **Pipeline Gráfico:** Configuración de Universal Render Pipeline (URP).
- [ ] **Iluminación 2D:** 
    - [ ] Luces puntuales en Void y fragmentos.
    - [ ] Sistema de Global Volume para post-procesado (Bloom, Vignette, Fog).
- [ ] **Shaders:**
    - [ ] Shader de disolución para muerte/teletransporte.
    - [ ] Efecto de distorsión para el Dash.
- [ ] **Parallax:** Script de 4 capas de profundidad para el fondo del Umbral.

---

## 🏰 Fase 5: Producción de Contenido (Niveles)
*Objetivo: Diseñar los niveles de los 3 Actos.*

- [ ] **Nivel 0 (Tutorial):** El Jardín de los Ecos (Enseñanza de mecánicas).
- [ ] **Acto 1:** El Despertar (Niveles 1-5).
- [ ] **Acto 2:** El Abismo (Niveles 6-12) + Introducción de Cazadores.
- [ ] **Acto 3:** El Retorno (Niveles 13-Final) + Mecánicas de gravedad/viento.
- [ ] **Level Design:** Greyboxing -> Scriptado de eventos -> Decoración visual.

---

## 📱 Fase 6: Pulido, UI y Audio
*Objetivo: Crear una experiencia premium y responsiva.*

- [ ] **UI (Ethereal Glass):**
    - [ ] Main Menu, Pause Menu y Level Select.
    - [ ] HUD minimalista (Contador de fragmentos y brillo de Dash).
- [ ] **Audio Adaptativo:**
    - [ ] Capas de música vertical (Exploración vs Tensión).
    - [ ] SFX para cada paso según superficie.
- [ ] **Dificultad:** Implementar selección de modos (Historia, Normal, Difícil).

---

## 🚀 Fase 7: Optimización y Exportación
*Objetivo: Rendimiento estable en todas las plataformas.*

- [ ] **Mobile:** Configuración de Joysticks virtuales y optimización de draw calls.
- [ ] **PC:** Soporte para resoluciones ultra-wide y re-vínculo de teclas.
- [ ] **Build Pipeline:** Automatización de builds para Android/Windows.

---

## ✅ Checklist de Validación Final
- [ ] ¿El control de Void se siente "responsivo y ligero"?
- [ ] ¿Están presentes los 3 coleccionables en cada nivel?
- [ ] ¿La curva de dificultad produce el estado de "Flow"?
- [ ] ¿La interfaz es legible en pantallas móviles pequeñas?
- [ ] ¿El audio refuerza la melancolía del Umbral?
