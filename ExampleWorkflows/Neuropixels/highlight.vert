#version 400
uniform float yshift = 0.0;
uniform float ywindowzoom = 1;
in vec2 vp;
in vec2 vt;
out vec2 tex_coord; 

void main()
{
  float yscale = ywindowzoom >= 1 ? ywindowzoom : 1.0;
  vec2 scale = vec2(0.333, 1.0 / yscale);
  vec2 shift = vec2(-0.6667, yshift);
  gl_Position = vec4(vp * scale + shift, 0.2, 1.0);
  tex_coord = vt;
}
