-- Fix images to match product names

-- ID 3: Canga Toalha - was showing beach landscape, now beach towel
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1600369672770-985fd30004eb?w=500&q=80' WHERE id = 3;

-- ID 4: Guarda-Sol Praia - was generic beach, now beach umbrella
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1531256456869-ce942a665e80?w=500&q=80' WHERE id = 4;

-- ID 8: Boardshort - was beach landscape, now board shorts
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=500&q=80' WHERE id = 8;
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1506629082955-511b1aa562c8?w=500&q=80' WHERE id = 8;

-- ID 12: Boia Inflavel - was ocean waves, now inflatable float
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1560089000-7433a4ebbd64?w=500&q=80' WHERE id = 12;

-- ID 13: Barraca Camping 6P - was campfire scene, now actual tent
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1478131143081-80f7f84ca84d?w=500&q=80' WHERE id = 13;

-- ID 17: Colchonete Inflavel - was tent scene, now sleeping pad/mat
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1445308394109-4ec2920981b1?w=500&q=80' WHERE id = 17;

-- ID 21: Rede com Mosquiteiro - was resort pool, now hammock
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1474552226712-ac0f0961a954?w=500&q=80' WHERE id = 21;

-- ID 27: Mochila Anti-Furto - was same as mala de bordo, now anti-theft backpack
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1622260614153-03223fb72052?w=500&q=80' WHERE id = 27;

-- ID 29: Adaptador de Tomada - was headphones, now travel adapter
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1621600411688-4571f93b56e0?w=500&q=80' WHERE id = 29;

-- ID 30: Travesseiro Cervical - was tissue/pillow generic, now neck pillow
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1592789705501-f9ae4287c4e9?w=500&q=80' WHERE id = 30;

-- ID 31: Organizador de Malas - same image as necessaire, now packing cubes
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=500&q=80' WHERE id = 31;

-- ID 34: Doleira RFID - was travel landscape, now money belt/pouch
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1548036328-c9fa89d128fa?w=500&q=80' WHERE id = 34;

-- ID 36: Pochete Esportiva - was backpack, now fanny pack
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1556306535-0f09a537f0a3?w=500&q=80' WHERE id = 36;

-- ID 40: Lanterna Frontal - was blurry/abstract, now headlamp
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1495954484750-af469f2f9be5?w=500&q=80' WHERE id = 40;

-- ID 42: Binoculos - was camera, now binoculars
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1509281373149-e957c6296406?w=500&q=80' WHERE id = 42;

-- ID 44: Cadeira de Praia Reclinavel (ambos) - was beach landscape, now beach chair
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1531722569936-825d3dd91b15?w=500&q=80' WHERE id = 44;

-- ID 48: Power Bank Solar - was generic
UPDATE produtos SET imagem = 'https://images.unsplash.com/photo-1585338107529-13afc5f02586?w=500&q=80' WHERE id = 48;
