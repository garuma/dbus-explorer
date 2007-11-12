(module "csharp" 
	 (aliases (("Int32" "int")
		   ("Int64" "long")
		   ("UInt32" "uint")
		   ("UInt64" "ulong")
		   ("Single" "float")
		   ("Double" "double")) ) 
	 ; first argument is the general style of the parsing, second is the agencement of parameters
	 ; third is the accumulator that will be placed between each arg block defined in 2, fourth and
	 ; fifth are respectively something that will be placed at the beginning of the first arg block
	 ; and something that will be placed at the end of the last arg block
	 (method-syntax "%{return} %{name}(%{args})" "%{arg-type} {arg-name}" ", " "" "")
	 (event-syntax "event EventHandler<%{type}> %{name};" "%{type}" ", " "" "")
	 (property-syntax "%{type} %{name} { %{replacement} }" "get;" "set;" "get; set;")

	 (struct-definition "struct { %{replacement} }" "{type}" ", " "" "")
	 (dictionary-definition "Dictionary<%{replacement}>" "%{type}" ", " "" "")
	 (array-definition "{type}[]")
)