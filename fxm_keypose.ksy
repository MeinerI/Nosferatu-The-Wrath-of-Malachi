meta:
  id: key_pose_fxm
  file-extension: fxm
  endian: le

seq:
  - id: dva
    type: u4

  - id: submeshes1
    type: u4

  - id: joint_count
    type: u4

  - id: zero8
    size: 8

  - id: joints
    type: joint
    repeat: expr
    repeat-expr: joint_count

  - id: zeros9
    type: u4
    repeat: expr
    repeat-expr: 9

################################

  - id: submeshes2
    type: u4

  - id: subset_name
    type: block_name

  - id: zero6
    type: u4
    repeat: expr
    repeat-expr: 6

################################

  - id: face_count
    type: u4

  - id: vertex_count
    type: u4

  - id: faces
    type: face
    repeat: expr
    repeat-expr: face_count

  - id: vertex
    type: vertex
    repeat: expr
    repeat-expr: vertex_count

# далее идут значения весов
# хз как они читаются

################################

types:

  block_name: 
    seq:
      - id: str_len
        type: u4
      - id: string
        type: str
        size: str_len
        encoding: ascii

  joint:
    seq:
      - id: joint_name
        type: block_name
      - id: nesting_depth
        type: u4
      - id: matrix4x4
        type: matrix4x4

      - id: zero8
        size: 8

  matrix4x4:
    seq:
      - id: row
        type: row
        repeat: expr
        repeat-expr: 4

  row:
    seq:
      - id: seq
        type: f4
        repeat: expr
        repeat-expr: 4


  face: 
    seq:
      - id: point_index
        type: u2
        repeat: expr
        repeat-expr: 3

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

      - id: u
        type: f4
      - id: v
        type: f4
