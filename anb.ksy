meta:
  id: anb
  file-extension: anb
  endian: le

seq:

  - id: header
    type: header

  - id: meshes
    type: mesh
    repeat: expr
    repeat-expr: header.submesh_count
    
  - id: sets
    type: set
    repeat: expr
    repeat-expr: header.set_count-1

types:

  header:
    seq:
      - id: set_count
        type: u4
      - id: submesh_count
        type: u4
      - id: vertex_count
        type: u4
      - id: uvs_count
        type: u4
      - id: face_count
        type: u4

  mesh:
    seq:  
      - id: vertices
        type: vertex
        repeat: expr
        repeat-expr: _root.header.vertex_count
      - id: faces
        type: face
        repeat: expr
        repeat-expr: _root.header.face_count
      - id: texture_coord
        type: texcoord
        repeat: expr
        repeat-expr: _root.header.uvs_count
      - id: faces_2
        type: face
        repeat: expr
        repeat-expr: _root.header.face_count

  set:
    seq:
      - id: vertices
        type: vertex
        repeat: expr
        repeat-expr: _root.header.vertex_count

  vertex:
    seq:
      - id: x 
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4
      - id: n1
        type: f4
      - id: n2
        type: f4
      - id: n3
        type: f4

  face:
    seq:
      - id: v1
        type: u2
      - id: v2
        type: u2
      - id: v3
        type: u2

  texcoord: 
    seq:
      - id: s
        type: f4
      - id: t
        type: f4
