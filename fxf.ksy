# для FXLibrary.fxf файла
#########################

meta:
  id: fxf
  file-extension: fxf
  endian: le

#########################
seq:
#########################

  - id: hz1
    type: u4

  - id: hz2
    type: f4

  - id: zero___00___16
    size: 16

#########################

  - id: big_folder_block_count
    type: u4
    
  - id: big_blocks
    type: big_folder_block
    repeat: expr
    repeat-expr: big_folder_block_count

#########################

  - id: count_entries
    type: u4

#########################

  - id: block_ref_sound
    type: ref_sound_block
  - id: block_ref_texture
    type: ref_texture_block
  - id: block_ref_mesh
    type: ref_mesh_block
  - id: block_ref_material
    type: ref_mat_block

  - id: all1
    type: all
    repeat: expr
    repeat-expr: 647

  - id: hzz1
    size: 4

  - id: all2
    type: all
    repeat: expr
    repeat-expr: 1624
    # где то рядом с 1225 первый font

  - id: hzz2
    size: 4

  - id: all3
    type: all
    repeat: expr
    repeat-expr: 102

  - id: hzz3
    size: 4

  - id: all4
    type: all
    repeat: expr
    repeat-expr: 9
  
  - id: hzz3_mat_4
    size: 170

  - id: all5
    type: all
    repeat: expr
    repeat-expr: 43
    
  - id: hzz3_mat_5
    size: 168

  - id: all6
    type: all
    repeat: expr
    repeat-expr: 63

  - id: hzz6
    size: 4

  - id: all7
    type: all
    repeat: expr
    repeat-expr: 283
  
  - id: hzz7
    size: 4

  - id: all8
    type: all
    repeat: expr
    repeat-expr: 712

  - id: hzz8
    size: 4

  - id: all9
    type: all
    repeat: expr
    repeat-expr: 157

  - id: font9
    size: 4153

  - id: all10
    type: all
    repeat: expr
    repeat-expr: 32

  - id: hz10
    size: 4

  - id: all11
    type: all
    repeat: expr
    repeat-expr: 32

#########################
types:
#########################

  all:
    seq:
      - id: block_type
        type: u4
        enum: block_type
      - id: block
        type: 
          switch-on: block_type
          cases:
            'block_type::texture':    texture_block   # 0 mpg тоже? 
            'block_type::fxm_or_anb': mesh_block      # 1
            'block_type::wav':        wav_block       # 2
            'block_type::material':   material_block  # 4
            'block_type::motion':     mot_block       # 6
            'block_type::font':       font_block      # 7

#########################

  big_folder_block:
    seq:
      - id: small_folder_block_count
        type: u4
      - id: small_folder_blocks
        type: small_folder_block
        repeat: expr
        repeat-expr: small_folder_block_count

#########################

  small_folder_block:
    seq:
      - id: len
        type: u4
      - id: name
        type: str
        size: len
        encoding: ASCII

#########################

  str_len:
    seq:
    - id: len
      type: u4
    - id: name
      type: str
      size: len
      encoding: ASCII

#########################

  block_3_name:
    seq:
      - id: number
        type: u4
      - id: zero
        size: 4
      - id: name
        type: str_len
        repeat: expr
        repeat-expr: 3

#########################

  ref_mesh_block: 
    seq:
      - id: block_type
        type: u4
      - id: name
        type: block_3_name
      - id: hz
        type: f4
        repeat: expr
        repeat-expr: 21

#########################

  ref_texture_block:
    seq:
     - id: block_type
       type: u4
     - id: name
       type: block_3_name
     - id: hz
       type: u4
       repeat: expr
       repeat-expr: 8

#########################

  ref_sound_block:
    seq:
      - id: block_type
        type: u4
      - id: name
        type: block_3_name
      - id: hz
        type: u4
        repeat: expr
        repeat-expr: 5

#########################

  ref_mat_block:
    seq:
      - id: block_type
        type: u4
      - id: number
        type: u4
      - id: zero
        size: 4
      - id: name
        type: str_len
      - id: hz
        size: 310

#########################

  material_block: # ref_37
    seq:
      - id: number
        type: u4
      - id: zero
        size: 4
      - id: name
        type: str_len

      - id: us4
        type: u4
        repeat: expr
        repeat-expr: 4

      - id: us7
        type: f4
        repeat: expr
        repeat-expr: 7

      - id: us5
        type: u4
        repeat: expr
        repeat-expr: 5

      - id: floats3
        type: f4
        repeat: expr
        repeat-expr: 3

      - id: us6
        type: u4
        repeat: expr
        repeat-expr: 6

      - id: reference_on_texture
        type: u4

      - id: floats4
        type: u4
        repeat: expr
        repeat-expr: 10

#########################

  texture_block:
    seq:
     - id: name
       type: block_3_name
     - id: hz
       type: u4
       repeat: expr
       repeat-expr: 6
     - id: height
       type: u4
     - id: width
       type: u4

#########################

  mot_block: 
    seq:
      - id: name
        type: block_3_name
      - id: hz
        type: u4
        repeat: expr
        repeat-expr: 3

#########################

  texture_plus_material_block:
    seq:
      - id: texture
        type: texture_block
      - id: material
        type: material_block

#########################

  wav_block: 
    seq:
      - id: name
        type: block_3_name
      - id: hz
        type: u4
        repeat: expr
        repeat-expr: 5

#########################

  mesh_block: # fxm_or_anb
    seq:
      - id: name
        type: block_3_name

      - id: zero1
        type: u4
        
      - id: zero2
        type: u4

      - id: counter_submeshes
        type: u4

      - id: hz
        type: float11
        repeat: expr
        repeat-expr: counter_submeshes

      - id: hz17
        type: u4
      - id: first_number_in_anb_file
        type: u4
        
#########################

  font_block:
    seq:
      - id: number
        type: u4
      - id: zero
        size: 4
      - id: block_name
        type: str_len
      - id: twelve_12_zero
        size: 12
      - id: font_name
        type: str_len
      - id: body
        size: 4111

#########################

  float11:
    seq:

      - id: hz1
        type: u4
      - id: ref_to_material
        type: u4

      - id: hz3
        type: f4
      - id: hz4
        type: f4
      - id: hz5
        type: f4

      - id: hz6
        type: f4
      - id: hz7
        type: f4
      - id: hz8
        type: f4

      - id: hz9
        type: f4
      - id: hz10
        type: f4
      - id: hz11
        type: f4

      - id: hz12
        type: f4
      - id: hz13
        type: f4

      - id: hz17
        type: u4
      - id: hz18
        type: u4
      - id: hz19
        type: u4

#########################

enums:
  block_type:
    0: texture
    1: fxm_or_anb
    2: wav
    4: material
    6: motion
    7: font
