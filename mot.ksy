################################################################################
meta:
################################################################################

  id: mot
  file-extension: mot
  endian: le

################################################################################
seq:
################################################################################

  - id: hz_1
    size: 12

  - id: float1
    type: f4

  - id: hz_2
    size: 12

  - id: node_count
    type: u4

  - id: nodes
    type: node
    repeat: expr
    repeat-expr: node_count

################################################################################
types:
################################################################################

  str_len:
    seq:
    - id: len
      type: u4
    - id: name
      type: str
      size: len
      encoding: ASCII

##############################

  node:
    seq:

    - id: hz_value_type
      type: f4  

    - id: node_name
      type: str_len

##############################

    - id: delimiter1
      size: 8

##############################

    - id: float4_1
      type: f4
      repeat: expr
      repeat-expr: 4

    - id: hz_f4_or_u4_2
      type: f4

    - id: float4_2
      type: f4
      repeat: expr
      repeat-expr: 4

##############################

    - id: delimiter2
      size: 8

##############################

    - id: float3_1
      type: f4
      repeat: expr
      repeat-expr: 3

    - id: hz_f4_or_u4_3
      type: f4

    - id: float3_2
      type: f4
      repeat: expr
      repeat-expr: 3

##############################

    - id: delimiter3
      size: 8

##############################
  
    - id: float3_3
      type: f4
      repeat: expr
      repeat-expr: 3

    - id: hz_f4_or_u4_4
      type: f4

    - id: float3_4
      type: f4
      repeat: expr
      repeat-expr: 3

##############################

    - id: delimiter4
      size: 4

################################################################################
