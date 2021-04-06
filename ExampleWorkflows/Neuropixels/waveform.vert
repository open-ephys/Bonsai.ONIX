#version 400
uniform int rows;
uniform int cols;

uniform float ywindowzoom = 1;
uniform float amplitudezoom = 20;
uniform float yshift = 0;

in float amplitude;

void main()
{
  int rowID = gl_VertexID / cols;
  int sampleID = gl_VertexID % cols;

  float xoffset = (3.0 / 2.0) * float(sampleID) / cols; // - 1;

  float yoffset = (2.0 * rowID + 1) / rows - 1;

  float ampmult = 0.0001 * float(amplitudezoom);

  vec2 vp = vec2(xoffset - 0.3333, yoffset - yshift + ampmult * amplitude);

  float yscale = ywindowzoom >= 1 ? ywindowzoom : 1.0;
  vec2 scale = vec2(1, yscale);

  gl_Position = vec4(vp * scale , 0.0, 1.0);
}
