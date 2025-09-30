class_name TestCustomMathGd
extends GdUnitTestSuite

# @warning_ignore('unused_parameter')




func test_in_range() -> void:
	var is_in_range_value : bool
	is_in_range_value = CustomMath.is_in_range(1, 0, 2,)
	assert_bool(is_in_range_value).is_true()
	
