# для FXLibrary.fxf файла

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

  - id: hz
    type: u4

  - id: block_ref_sound
    type: ref_sound_block

  - id: block_ref_texture
    type: ref_texture_block

  - id: block_ref_mesh
    type: ref_mesh_block

  - id: block_ref_material
    type: material_block

  - id: material1
    type: material_block

  - id: texture1
    type: texture_block

  - id: texture2
    type: texture_block

  - id: material2
    type: material_block

  - id: material3
    type: material_block

  - id: textures_plus_materials
    type: texture_plus_material_block
    repeat: expr
    repeat-expr: 41

  - id: motions
    type: mot_block
    repeat: expr
    repeat-expr: 7

  - id: texture3
    type: texture_block

  - id: motion1
    type: mot_block

  - id: material4
    type: material_block

  - id: motion2
    type: mot_block

  - id: texture4
    type: texture_block

  - id: motion3
    type: mot_block

  - id: material5
    type: material_block

  - id: texture_plus_material_block0
    type: texture_plus_material_block

  - id: texture6
    type: texture_block

  - id: wav_block1
    type: wav_block

  - id: wav_block2
    type: wav_block

  - id: wav_block3
    type: wav_block

  - id: texture7
    type: texture_block

  - id: wav_block6
    type: wav_block

  - id: wav_block7
    type: wav_block

  - id: texture8
    type: texture_block

  - id: motion9
    type: mot_block

  - id: mesh1
    type: fxm_mesh_block

  - id: textures_plus_materials2
    type: texture_plus_material_block
    repeat: expr
    repeat-expr: 23

  - id: mesh2
    type: fxm_mesh_block

  - id: textures_plus_materials3
    type: texture_plus_material_block

  - id: textures_plus_materials4
    type: texture_plus_material_block

  - id: mesh3
    type: fxm_mesh_block
  
  - id: textures_plus_materials5
    type: texture_plus_material_block

  - id: mesh4
    type: fxm_mesh_block

  - id: mesh5
    type: fxm_mesh_block

  - id: motion10
    type: mot_block



#########################
types:
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
#########################
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
#########################
#########################

  ref_mesh_block: 
    seq:
      - id: name
        type: block_3_name
      - id: hz
        type: u4
        repeat: expr
        repeat-expr: 22

#########################

  ref_texture_block:
    seq:
     - id: name
       type: block_3_name
     - id: hz
       type: u4
       repeat: expr
       repeat-expr: 9

#########################

  ref_sound_block:
    seq:
      - id: name
        type: block_3_name
      - id: hz
        type: u4
        repeat: expr
        repeat-expr: 6

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
        repeat-expr: 11

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
     - id: hz3
       type: u4

#########################

  mot_block: 
    seq:
      - id: name
        type: block_3_name
      - id: hz
        type: u4
        repeat: expr
        repeat-expr: 4

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
        repeat-expr: 6

#########################

  fxm_mesh_block: 
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
      - id: hz18
        type: u4
      - id: hz19
        type: u4

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
