Funcionalidade: Pedido - Adicionar item ao carrinho
	Como um usuário
	Eu desejo colocar um item no carrinho
	Para que eu possa comprá-lo posteriormente

Cenário: Adicionar item com sucesso a um novo pedido
Dado que o usuário esteja logado
E que um produto esteja na vitrine
E esteja disponível no estoque
E o carrinho esteja vazio
Quando o usuário adicionar uma unidade ao carrinho
Então o usuário será redirecionado ao resumo da compra
E o valor total do pedido será extatamente o valor do item adicionado

Cenário: Adicionar itens acima do limite
Dado que o usuário esteja logado
E aceita o uso de cookies
E que um produto esteja na vitrine
E esteja disponível no estoque
Quando o usuário adicionar um item acima da quantidade máxima permitida
Então receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite

Cenário: Adicionar item já existente no carrinho
Dado que o usuário esteja logado
E aceita o uso de cookies
E que um produto esteja na vitrine
E esteja disponível no estoque
E o mesmo produto tenha sido adicionado ao carrinho anteriormente
Quando o usuário adicionar uma unidade no pedido
Então o usuário será redirecionado ao resumo da compra
E a quantidade quantidade daquele produto será acrescida de uma unidade a mais
E o valor total do pedido será a multiplicação da quantidade de itens pelo valor unitário

Cenário: Adicionar item já existente no carrinho onde a soma ultrapassa o limite máximo
Dado que o usuário esteja logado
E aceita o uso de cookies
E que um produto esteja na vitrine
E esteja disponível no estoque
E o mesmo produto tenha sido adicionado ao carrinho anteriormente
Quando o usuário adicionar mais que a quantidade máxima permitida ao carrinho
Então receberá uma mensagem de erro mencionando que foi ultrapassada a quantidade limite
