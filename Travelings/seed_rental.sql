-- Produtos de ALUGUEL (tipo = 'aluguel')

-- Praia
INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Guarda-Sol Premium UV50+', 189.90, 'Guarda-sol com protecao UV50+, tecido resistente ao vento e base reforcada. Ideal para praias e piscinas.', 25, 'Praia', 'Protecao', 'https://images.unsplash.com/photo-1531256456869-ce942a665e80?w=500&q=80', 3, true, 'aluguel', 24.90, 1, 15, 20);

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Prancha de Surf Iniciante', 899.90, 'Prancha soft top ideal para iniciantes. Flutuacao extra e seguranca para primeiras ondas.', 8, 'Praia', 'Diversao', 'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=500&q=80', 5, true, 'aluguel', 79.90, 1, 5, 8);

-- Acampamento
INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Saco de Dormir Termico -5C', 349.90, 'Saco de dormir com isolamento termico ate -5C. Compacto e ultraleve para trilhas longas.', 18, 'Acampamento', 'Equipamento', 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80', 2, true, 'aluguel', 29.90, 2, 10, 15);

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Lanterna Frontal Recarregavel 600lm', 129.90, 'Lanterna de cabeca com 600 lumens, bateria recarregavel USB-C e resistencia a agua IPX6.', 25, 'Acampamento', 'Iluminacao', 'https://images.unsplash.com/photo-1493932484895-752d1471eab5?w=500&q=80', 6, true, 'aluguel', 12.90, 1, 7, 20);

-- Turismo
INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Camera GoPro Hero 12', 2499.90, 'Camera de acao 5.3K com estabilizacao HyperSmooth, a prova d agua ate 10m. Registre cada momento.', 6, 'Turismo', 'Eletronicos', 'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=500&q=80', 4, true, 'aluguel', 89.90, 1, 14, 6);

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Binoculos Compactos 10x42', 389.90, 'Binoculos com lentes multicamadas, campo de visao amplo e corpo emborrachado antiderrapante.', 10, 'Turismo', 'Acessorios', 'https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=500&q=80', 1, true, 'aluguel', 34.90, 1, 7, 8);


-- Produtos de AMBOS (tipo = 'ambos') - venda E aluguel

-- Praia
INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Kit Snorkel Profissional', 249.90, 'Kit completo com mascara panoramica, snorkel seco e nadadeiras ajustaveis. Perfeito para mergulho.', 15, 'Praia', 'Diversao', 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=500&q=80', 3, true, 'ambos', 34.90, 1, 7, 12);

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Cadeira de Praia Reclinavel', 159.90, 'Cadeira com 5 posicoes de recline, tecido impermeavel e estrutura em aluminio. Leve e compacta.', 30, 'Praia', 'Conforto', 'https://images.unsplash.com/photo-1519046904884-53103b34b206?w=500&q=80', 5, true, 'ambos', 14.90, 1, 30, 25);

-- Acampamento
INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Barraca Camping 4 Pessoas', 649.90, 'Barraca impermeavel com dupla camada, ventilacao cruzada e montagem rapida. Perfeita para aventuras em familia.', 10, 'Acampamento', 'Abrigo', 'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80', 2, true, 'ambos', 89.90, 2, 14, 8);

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Fogareiro Portatil a Gas', 199.90, 'Fogareiro compacto com regulagem de chama e ignicao automatica. Compativel com botijoes padrao.', 20, 'Acampamento', 'Cozinha', 'https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?w=500&q=80', 6, true, 'ambos', 19.90, 1, 7, 15);

-- Turismo
INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Mala de Viagem Rigida 28pol', 599.90, 'Mala com rodas 360, cadeado TSA integrado e interior organizador. Resistente a impactos.', 12, 'Turismo', 'Organizacao', 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80', 4, true, 'ambos', 49.90, 3, 30, 10);

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, subcategoria, imagem, idloja, ativo, tipo, precoaluguel, diasminimo, diasmaximo, quantidadedisponivel)
VALUES ('Power Bank Solar 20000mAh', 179.90, 'Carregador portatil com painel solar, 2 saidas USB e lanterna LED integrada. Nunca fique sem bateria.', 20, 'Turismo', 'Eletronicos', 'https://images.unsplash.com/photo-1609091839311-d5365f9ff1c5?w=500&q=80', 1, true, 'ambos', 14.90, 1, 14, 18);
