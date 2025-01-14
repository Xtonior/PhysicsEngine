#version 330 core

out vec4 FragColor;

uniform vec3 objectColor;

void main()
{
    FragColor = vec4(objectColor, 1.0); // Alpha is set to 1 for full opacity
}
