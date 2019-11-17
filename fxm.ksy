meta:
  id: fxm
  endian: le
  application: Nosferatu The wrath of Malachi
  file-extension: fxm

seq:

  - id: unknown0
    size: 44

  - id: submesh_count
    type: u4

  - id: submeshes
    type: submesh
    repeat: expr
    repeat-expr: submesh_count

types:

  submesh:
    seq:

      - id: texture_name_length
        type: u4
      - id: texture_name
        type: str
        size: texture_name_length
        encoding: ASCII

      - id: unknown1
        size: 24

      - id: face_count
        type: u4
    
      - id: vertex_count
        type: u4

      - id: faces
        type: face
        repeat: expr
        repeat-expr: face_count

      - id: vertices
        type: vertex
        repeat: expr
        repeat-expr: vertex_count

  face:
    seq:
      - id: vi0
        type: u2
      - id: vi1
        type: u2
      - id: vi2
        type: u2

  vertex:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4

      - id: vn1
        type: f4
      - id: vn2
        type: f4
      - id: vn3
        type: f4

      - id: u
        type: f4
      - id: v
        type: f4
