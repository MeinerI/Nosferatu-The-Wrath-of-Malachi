meta:
  id: key_pose_fxm
  file-extension: fxm
  endian: le

seq:
  - id: major_number
    type: u4
  - id: minor_number
    type: u4

  - id: joint_count
    type: u4

  - id: joints
    type: joint
    repeat: expr
    repeat-expr: joint_count
    if: (major_number == 2) and (minor_number == 1)

  - id: zeros9
    type: u4
    repeat: expr
    repeat-expr: 11

################################

  - id: submeshes2
    type: u4

  - id: subset_name
    type: mesh_name

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

  - id: vertices
    type: vertex
    repeat: expr
    repeat-expr: vertex_count

  - id: vertexy
    type: vertex_weigth_color
    repeat: expr
    repeat-expr: vertex_count

################################

types:

  mesh_name: 
    seq:
      - id: str_len
        type: u4
      - id: string
        type: str
        size: str_len
        encoding: ascii

  joint:
    seq:
      - id: delimetr
        size: 8
      - id: joint_name
        type: mesh_name
      - id: number_parent_bone
        size: 4
      - id: matrix4x4
        type: matrix4x4

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

  null0:
    seq:
      - id: weigth
        type: f4

      - id: color_argb_hz1 # от 0 до 255
        type: u1
      - id: hz1 # от 0 до 9
        type: u1
      - id: zero11 # обычно 0, очень редко FF FF
        type: u1
      - id: zero12 # обычно 0, очень редко FF FF
        type: u1

      - id: color_argb_hz2
        type: u1
      - id: hz2
        type: u1
      - id: zero21
        type: u1
      - id: zero22
        type: u1

      - id: color_argb_hz3
        type: u1
      - id: hz3
        type: u1
      - id: zero31
        type: u1
      - id: zero32
        type: u1

      - id: color_argb_hz4
        type: u1
      - id: hz4
        type: u1
      - id: zero41
        type: u1
      - id: zero42
        type: u1

  null2:
    seq:
      - id: weigth1
        type: f4
      - id: weigth2
        type: f4

      - id: color_argb_hz1
        type: u1
      - id: hz1
        type: u1
      - id: zero11
        type: u1
      - id: zero12
        type: u1

      - id: color_argb_hz2
        type: u1
      - id: hz2
        type: u1
      - id: zero21
        type: u1
      - id: zero22
        type: u1

      - id: color_argb_hz3
        type: u1
      - id: color_argb_hz4
        type: u1
      - id: zero31
        type: u1
      - id: zero32
        type: u1

        
  null3:
    seq:
      - id: weigth1
        type: f4
      - id: weigth2
        type: f4
      - id: weigth3
        type: f4

      - id: color_argb_hz1
        type: u1
      - id: hz1
        type: u1
      - id: zero11
        type: u1
      - id: zero12
        type: u1

      - id: color_argb_hz2
        type: u1
      - id: color_argb_hz3
        type: u1
      - id: color_argb_hz4
        type: u1
      - id: zero
        type: u1

  null4:
    seq:
      - id: weigth1
        type: f4
      - id: weigth2
        type: f4
      - id: weigth3
        type: f4
      - id: weigth4
        type: f4

      - id: color_argb_hz1
        type: u1
      - id: color_argb_hz2
        type: u1
      - id: color_argb_hz3
        type: u1
      - id: color_argb_hz4
        type: u1

  vertex_weigth_color:
    seq:
      - id: x
        type: f4
      - id: y
        type: f4
      - id: z
        type: f4
        
      - id: b1
        type: u1
      - id: b2
        type: u1
      - id: b3
        type: u1
      - id: b4
        type: u1
      
      - id: null_0
        type: null0
        if: b1 == 0

      - id: null_2
        type: null2
        if: b1 == 2

      - id: null_3
        type: null3
        if: b1 == 3

      - id: null_4
        type: null4
        if: b1 == 4

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
