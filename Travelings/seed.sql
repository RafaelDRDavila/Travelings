-- ════════════════════════════════════════════════════════
--  SEED: Lojas com CNPJs reais de empresas brasileiras
-- ════════════════════════════════════════════════════════

-- Atualizar Decathlon (id=8)
UPDATE lojas SET
  descricao = 'Tudo para o esporte e aventura. Equipamentos para todos os niveis.',
  logo = 'https://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Decathlon_Logo.svg/512px-Decathlon_Logo.svg.png',
  banner = 'https://images.unsplash.com/photo-1551632811-561732d1e306?w=1400&q=80',
  email = 'contato@decathlon.com.br',
  telefone = '(11) 4003-7424',
  endereco = 'Sao Paulo - SP'
WHERE id = 8;

-- Nautika Lazer (CNPJ: Nautika)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Nautika Lazer',
  '50872833000120',
  'Referencia em equipamentos para camping, pesca e aventura desde 1993.',
  'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=200&q=80',
  'https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=1400&q=80',
  'sac@nautika.com.br',
  '(11) 2065-9800',
  'Guarulhos - SP'
);

-- Havaianas (CNPJ: Alpargatas S.A.)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Havaianas',
  '61079117000105',
  'Nascida no Brasil, amada no mundo. Chinelos, sandalias e acessorios para o verao.',
  'https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Havaianas_logo.svg/512px-Havaianas_logo.svg.png',
  'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1400&q=80',
  'faleconosco@havaianas.com.br',
  '(11) 3797-0222',
  'Sao Paulo - SP'
);

-- Samsonite (CNPJ real)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Samsonite',
  '02802609000108',
  'Lider mundial em malas e acessorios de viagem desde 1910. Qualidade e durabilidade.',
  'https://upload.wikimedia.org/wikipedia/commons/thumb/a/a1/Samsonite_logo.svg/512px-Samsonite_logo.svg.png',
  'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=1400&q=80',
  'sac@samsonite.com.br',
  '(11) 3504-8888',
  'Sao Paulo - SP'
);

-- Quiksilver (CNPJ: Boardriders Brasil)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Quiksilver',
  '06057223000171',
  'Surf, neve e outdoor. Roupas e equipamentos para quem vive a natureza.',
  'https://upload.wikimedia.org/wikipedia/commons/thumb/b/b5/Quiksilver_logo.svg/512px-Quiksilver_logo.svg.png',
  'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=1400&q=80',
  'atendimento@quiksilver.com.br',
  '(11) 3047-4700',
  'Sao Paulo - SP'
);

-- Trilhas & Rumos (CNPJ: Centauro)
INSERT INTO lojas (nome, cnpj, descricao, logo, banner, email, telefone, endereco)
VALUES (
  'Trilhas e Rumos',
  '06312878000107',
  'Especializada em trekking, montanhismo e viagens de aventura.',
  'https://images.unsplash.com/photo-1551632811-561732d1e306?w=200&q=80',
  'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1400&q=80',
  'contato@trilhaserumos.com.br',
  '(11) 3062-8100',
  'Sao Paulo - SP'
);

-- ════════════════════════════════════════════════════════
--  Buscar IDs das lojas para usar nos produtos
-- ════════════════════════════════════════════════════════
-- Vamos usar subqueries com nome da loja

-- ════════════════════════════════════════════════════════
--  PRODUTOS - PRAIA (diversificados entre lojas)
-- ════════════════════════════════════════════════════════

INSERT INTO produtos (nome, preco, descricao, estoque, categoria, imagem, idloja, ativo) VALUES
-- Havaianas
('Chinelo Havaianas Top', 39.90, 'O classico chinelo brasileiro. Confortavel, duravel e com estilo para a praia.', 50, 'Praia',
 'https://images.unsplash.com/photo-1603487742131-4160ec999306?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

('Sandalia Havaianas Slim', 54.90, 'Design feminino e elegante. Tiras finas e acabamento premium.', 35, 'Praia',
 'https://images.unsplash.com/photo-1562183241-b937e95585b6?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

-- Quiksilver
('Boardshort Quiksilver Highline', 249.90, 'Bermuda de surf com tecido 4-way stretch e secagem rapida. Perfeita para ondas.', 25, 'Praia',
 'https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Rashguard UV50+ Quiksilver', 179.90, 'Camiseta de lycra com protecao solar UV50+. Ideal para surf e esportes aquaticos.', 30, 'Praia',
 'https://images.unsplash.com/photo-1544551763-46a013bb70d5?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

('Bone Quiksilver Trucker', 129.90, 'Bone com tela traseira e ajuste snapback. Protecao e estilo na praia.', 40, 'Praia',
 'https://images.unsplash.com/photo-1588850561407-ed78c334e67a?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06057223000171'), true),

-- Decathlon
('Guarda-Sol Praia UV40', 149.90, 'Guarda-sol com protecao UV40, estrutura em aluminio e bolsa de transporte.', 20, 'Praia',
 'https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=500&q=80',
 8, true),

('Snorkel Kit Adulto', 89.90, 'Kit completo com mascara de silicone e snorkel. Lente anti-embacante.', 30, 'Praia',
 'https://images.unsplash.com/photo-1544551763-77932f56a0ec?w=500&q=80',
 8, true),

('Cadeira de Praia Reclinavel', 119.90, 'Cadeira com 5 posicoes, tecido impermeavel e estrutura reforçada em aluminio.', 25, 'Praia',
 'https://images.unsplash.com/photo-1473116763249-2faaef81ccda?w=500&q=80',
 8, true),

('Prancha Bodyboard 42"', 159.90, 'Prancha de bodyboard em EPS com slick e leash incluso. Para iniciantes e intermediarios.', 15, 'Praia',
 'https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=500&q=80',
 8, true),

('Bolsa Termica Praia 20L', 79.90, 'Bolsa termica impermeavel com isolamento de 6 horas. Comporta ate 20 latas.', 35, 'Praia',
 'https://images.unsplash.com/photo-1519046904884-53103b34b206?w=500&q=80',
 8, true),

('Protetor Solar FPS 70 200ml', 49.90, 'Protecao solar de alta performance, resistente a agua. Para peles sensiveis.', 60, 'Praia',
 'https://images.unsplash.com/photo-1532274402911-5a369e4c4bb5?w=500&q=80',
 8, true),

('Canga Toalha Microfibra', 69.90, 'Canga 2 em 1: toalha de secagem rapida e canga de praia. Compacta e leve.', 45, 'Praia',
 'https://images.unsplash.com/photo-1506953823976-52e1fdc0149a?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='61079117000105'), true),

-- ════════════════════════════════════════════════════════
--  PRODUTOS - ACAMPAMENTO
-- ════════════════════════════════════════════════════════

-- Nautika
('Barraca Nautika Cherokee 6', 599.90, 'Barraca para 6 pessoas com coluna d''agua de 2000mm. Dupla camada e sobreteto.', 12, 'Acampamento',
 'https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Lanterna LED Recarregavel 1000lm', 89.90, 'Lanterna tatica com 1000 lumens, zoom ajustavel e bateria recarregavel USB-C.', 40, 'Acampamento',
 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Colchonete Inflavel Nautika', 149.90, 'Colchonete autoinflavel com espuma expandida. Isolamento termico R3.', 25, 'Acampamento',
 'https://images.unsplash.com/photo-1478131143081-80f7f84ca84d?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

('Fogareiro Compacto a Gas', 119.90, 'Fogareiro portatil com ignicao automatica. Compativel com cartuchos padrao.', 20, 'Acampamento',
 'https://images.unsplash.com/photo-1517824806704-9040b037703b?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='50872833000120'), true),

-- Trilhas e Rumos
('Mochila Cargueira 60L', 449.90, 'Mochila de trekking com quadro ajustavel, capa de chuva integrada e compartimento para saco de dormir.', 15, 'Acampamento',
 'https://images.unsplash.com/photo-1622260614153-03223fb72052?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Kit Panelas Camping Aluminio', 129.90, 'Conjunto com 2 panelas, frigideira e tampa multiuso. Revestimento antiaderente.', 30, 'Acampamento',
 'https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Canivete Multiuso 12 Funcoes', 79.90, 'Canivete em aco inox com faca, serra, abridor, chave phillips e mais 8 ferramentas.', 50, 'Acampamento',
 'https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Rede de Descanso Camping', 99.90, 'Rede de nylon ripstop com mosquiteiro integrado. Suporta ate 150kg.', 20, 'Acampamento',
 'https://images.unsplash.com/photo-1520250497591-112f2f40a3f4?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

-- Decathlon
('Saco de Dormir Confort 10°C', 199.90, 'Saco de dormir com temperatura de conforto de 10 graus. Enchimento sintetico lavavel.', 20, 'Acampamento',
 'https://images.unsplash.com/photo-1537905569824-f89f14cceb68?w=500&q=80',
 8, true),

('Cadeira Dobravel Camping', 99.90, 'Cadeira compacta com porta-copos e bolsa de transporte. Suporta ate 110kg.', 30, 'Acampamento',
 'https://images.unsplash.com/photo-1525811902-f2342640856e?w=500&q=80',
 8, true),

('Lampiao LED Solar Recarregavel', 69.90, 'Lampiao com painel solar integrado e USB. 3 modos de iluminacao, ate 30h de autonomia.', 35, 'Acampamento',
 'https://images.unsplash.com/photo-1510312305653-8ed496efae75?w=500&q=80',
 8, true),

-- ════════════════════════════════════════════════════════
--  PRODUTOS - TURISMO
-- ════════════════════════════════════════════════════════

-- Samsonite
('Mala de Viagem Samsonite Spinner 68cm', 899.90, 'Mala media com 4 rodas giratoria 360 graus, cadeado TSA integrado e interior organizado.', 15, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Mala de Bordo Samsonite 55cm', 649.90, 'Mala de mao aprovada por companhias aereas. Policarbonato ultra-leve e resistente.', 20, 'Turismo',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Necessaire Organizadora Samsonite', 149.90, 'Necessaire com divisorias internas, gancho para pendurar e material impermeavel.', 40, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

('Pochete Anti-Furto Samsonite', 199.90, 'Pochete com bloqueio RFID, ziper oculto e alca ajustavel. Seguranca para viagens.', 30, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='02802609000108'), true),

-- Decathlon
('Mochila Urbana Anti-Furto 25L', 199.90, 'Mochila com abertura traseira anti-furto, porta USB e compartimento para notebook 15".', 25, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 8, true),

('Garrafa Termica 750ml', 69.90, 'Garrafa de aco inox dupla parede. Mantem bebida gelada 24h ou quente 12h.', 50, 'Turismo',
 'https://images.unsplash.com/photo-1516035069371-29a1b244cc32?w=500&q=80',
 8, true),

('Adaptador Universal de Tomada', 59.90, 'Adaptador para mais de 150 paises com 2 portas USB e 1 USB-C. Compacto e seguro.', 60, 'Turismo',
 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=500&q=80',
 8, true),

('Travesseiro de Viagem Memory Foam', 89.90, 'Travesseiro cervical em espuma viscoelastica com capa lavavel e bolsa compacta.', 40, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 8, true),

-- Trilhas e Rumos
('Organizador de Malas 6 Pecas', 79.90, 'Kit com 6 sacos organizadores de diferentes tamanhos. Roupas, sapatos e acessorios separados.', 35, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Cadeado TSA com Segredo', 39.90, 'Cadeado aprovado pela TSA com combinacao de 3 digitos. Para malas e mochilas.', 80, 'Turismo',
 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Tag de Bagagem GPS', 129.90, 'Rastreador Bluetooth para malas. Localiza sua bagagem pelo app em tempo real.', 25, 'Turismo',
 'https://images.unsplash.com/photo-1565026057447-bc90a3dceb87?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true),

('Doleira de Viagem RFID', 49.90, 'Porta-documentos discreto para usar sob a roupa. Bloqueio RFID contra clonagem.', 55, 'Turismo',
 'https://images.unsplash.com/photo-1488646953014-85cb44e25828?w=500&q=80',
 (SELECT id FROM lojas WHERE cnpj='06312878000107'), true);
