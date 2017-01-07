#Shadows of Her Past - Spectra Peripherals
*Utilizing Colore Unity plugin for Razer Chroma. Version 1 focuses upon Keyboards*

##Methods
**chroma_control(string color)**
The main constructor for creating an object oriented control.
Color is the default color state.

**trigger_alarm(string color = Red)**
Triggers an alert effect causing a full keyboard flash of the chosen color.
This method defaults to flashing red.

**set_color(string key, string color)**
Sets the specified key to the designated color.

**set_fade(string key, string color1, string color2, int time)**
Activates a fade of the designated key from color1 to color2 during the given time.

**trigger_notif(string key, string color)**
Triggers a notification like effect. The given key will pulsate with the supplied color.
