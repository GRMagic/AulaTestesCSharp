insert into Categorias (Id, Nome, Codigo) Values (NEWID(), 'Canecas', 1)
insert into Categorias (Id, Nome, Codigo) Values (NEWID(), 'Camisas', 2)

insert into Produtos (Id, CategoriaId, Nome, Descricao, Ativo, Valor, DataCadastro, Imagem, QuantidadeEstoque, Altura, Largura, Profundidade) values 
(newid(), (select id from Categorias where (Codigo = 2)), 'Camiseta 1', 'Software Developer', 1, 29, GETDATE(), 'camiseta1.jpg', 10, 1,2,3),
(newid(), (select id from Categorias where (Codigo = 2)), 'Camiseta 2', 'Code Coffee Repeat Preta', 1, 25, GETDATE(), 'camiseta2.jpg', 10, 1,2,3),
(newid(), (select id from Categorias where (Codigo = 2)), 'Camiseta 3', 'Code Coffee Repeat Cinza', 1, 30, GETDATE(), 'camiseta3.jpg', 10, 1,2,3),
(newid(), (select id from Categorias where (Codigo = 2)), 'Camiseta 4', 'Debugar x Programar', 1, 15, GETDATE(), 'camiseta4.jpg', 10, 1,2,3),

(newid(), (select id from Categorias where (Codigo = 1)), 'Caneca 1', 'Starbugs coffee', 1, 45, GETDATE(), 'caneca1.jpg', 10, 1,2,3),
(newid(), (select id from Categorias where (Codigo = 1)), 'Caneca 2', 'Programmer Code', 1, 12, GETDATE(), 'caneca2.jpg', 10, 1,2,3),
(newid(), (select id from Categorias where (Codigo = 1)), 'Caneca 3', 'I turn coffee into code', 1, 53, GETDATE(), 'caneca3.jpg', 10, 1,2,3),
(newid(), (select id from Categorias where (Codigo = 1)), 'Caneca 4', 'No Coffee, No Code', 1, 10, GETDATE(), 'caneca4.jpg', 10, 1,2,3)

select * from Produtos