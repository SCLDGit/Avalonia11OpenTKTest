//Here we specify the version of our shader.
#version 330 core
//These lines specify the location and type of our attributes,
//the attributes here are prefixed with a "v" as they are our inputs to the vertex shader
//this isn't strictly necessary though, but a good habit.
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec4 vColor;

const vec2 quadVertices[4] = vec2[]( vec2(-1.0, -1.0), vec2(-1.0, 1.0), vec2(1.0, 1.0), vec2(1.0, -1.0) );

void main()
{
    gl_Position = vec4(quadVertices[gl_VertexID], 0.0, 1.0);
}