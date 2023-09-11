#version 330 core

out vec4 FragColor;

uniform vec2 resolution;
uniform float time;

float segment(vec2 p, float angle) {
    float a = atan(p.y, p.x) / angle;
    return mod(a, 2.0) < 1.0 ? length(p) : 0.0;
}

vec2 rotate(vec2 v, float a) {
    mat2 rot = mat2(cos(a), -sin(a),
    sin(a),  cos(a));
    return rot * v;
}

float noise(vec2 st) {
    vec2 ip = floor(st);
    vec2 u = fract(st);
    u = u*u*(3.0-2.0*u);

    float res = mix(
    mix(dot(ip, vec2(127.1,311.7)),
    dot(ip + vec2(0.0,1.0), vec2(127.1,311.7)), u.y),
    mix(dot(ip + vec2(1.0,0.0), vec2(127.1,311.7)),
    dot(ip + vec2(1.0,1.0), vec2(127.1,311.7)), u.y), u.x);

    return fract(res);
}

vec2 ripple(vec2 uv, float time) {
    // Two sine waves with different frequencies and directions to simulate rippling water effect
    float dx = sin(uv.y * 10.0 + time);
    float dy = sin(uv.x * 15.0 + time);
    return uv + vec2(dx, dy) * (sin(time) * 0.25 + 2.5);  // 0.03 controls the amplitude of the distortion. Adjust as needed.
}

void main() {
    vec2 uv = gl_FragCoord.xy / resolution;

    // Apply the ripple distortion
    uv = ripple(uv, time);

    vec2 p = uv - vec2(0.5);
    p = rotate(p, time);

    float numSegments = 8.0 + (sin(time) * 2.0 + 2.0);
    float angle = 6.28 / numSegments;
    //float edgeNoise = noise(p * (sin(time) * 0.01 + 0.15)); // * (sin(time) * 0.25 + 0.5);

    float r = segment(p, angle);

    float shift = sin(time + p.x * 5.0) * 0.5 + 0.5;
    
    // Dynamic color modulation
    vec3 baseColor = vec3(
    0.5 + 0.5 * sin((time * 0.25) * 0.7),
    0.5 + 0.5 * sin((time * 0.5) * 0.8 + 2.0),
    0.5 + 0.5 * sin((time * 0.35) * 0.9 + 4.0)
    );

    vec3 altColor = vec3(
    0.5 + 0.5 * cos((time * 0.125) * 0.6 + 1.0),
    0.5 + 0.5 * cos((time * 0.45) * 0.7 + 3.0),
    0.5 + 0.5 * cos((time * 0.55) * 0.8 + 5.0)
    );

    vec3 col = mix(baseColor, altColor, r + shift);
    FragColor = vec4(col, 1.0);
}
