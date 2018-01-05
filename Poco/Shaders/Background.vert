#version 330

uniform mat4 projection;
in vec2 vertex;
in vec2 coord;
out vec2 _coord;

void main()
{
    gl_Position = projection * vec4(vertex, 1.0, 1.0);
    _coord = coord;
}
