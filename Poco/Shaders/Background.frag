#version 330

uniform sampler2D texture;
in vec2 _coord;
out vec4 fragment_color;

void main()
{
    fragment_color = texture2D(texture, _coord);
}
