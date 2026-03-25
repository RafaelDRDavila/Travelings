#!/bin/bash
API="http://localhost:5260/api/v1"
H="Content-Type: application/json"

# ════════════════════════════════════════════════
#  LOJAS (CNPJs reais de empresas conhecidas)
# ════════════════════════════════════════════════

# Decathlon já existe (id=8), vamos atualizar
curl -s -X PUT "$API/lojas?id=8" -H "$H" -d '{
  "id":8,"nome":"Decathlon","cnpj":"02314041000202",
  "descricao":"Tudo para o esporte e aventura. Equipamentos acessíveis para todos os níveis.",
  "logo":"https://upload.wikimedia.org/wikipedia/commons/thumb/c/c1/Decathlon_Logo.svg/512px-Decathlon_Logo.svg.png",
  "banner":"https://images.unsplash.com/photo-1551632811-561732d1e306?w=1400&q=80",
  "email":"contato@decathlon.com.br","telefone":"(11) 4003-7424","endereco":"São Paulo - SP"
}'

echo ""

# Quechua (marca Decathlon outdoor) — CNPJ Iguatemi
curl -s -X POST "$API/lojas" -H "$H" -d '{
  "id":0,"nome":"Nautika Lazer","cnpj":"50872833000120",
  "descricao":"Referência em equipamentos para camping, pesca e aventura desde 1993.",
  "logo":"https://images.unsplash.com/photo-1504280390367-361c6d9f38f4?w=200&q=80",
  "banner":"https://images.unsplash.com/photo-1504851149312-7a075b496cc7?w=1400&q=80",
  "email":"sac@nautika.com.br","telefone":"(11) 2065-9800","endereco":"Guarulhos - SP"
}'
echo ""

# Havaianas — CNPJ Alpargatas
curl -s -X POST "$API/lojas" -H "$H" -d '{
  "id":0,"nome":"Havaianas","cnpj":"61079117000105",
  "descricao":"Nascida no Brasil, amada no mundo. Chinelos, sandálias e acessórios para o verão.",
  "logo":"https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Havaianas_logo.svg/512px-Havaianas_logo.svg.png",
  "banner":"https://images.unsplash.com/photo-1507525428034-b723cf961d3e?w=1400&q=80",
  "email":"faleconosco@havaianas.com.br","telefone":"(11) 3797-0222","endereco":"São Paulo - SP"
}'
echo ""

# Samsonite — CNPJ real
curl -s -X POST "$API/lojas" -H "$H" -d '{
  "id":0,"nome":"Samsonite","cnpj":"02802609000108",
  "descricao":"Líder mundial em malas e acessórios de viagem desde 1910. Qualidade e durabilidade.",
  "logo":"https://upload.wikimedia.org/wikipedia/commons/thumb/a/a1/Samsonite_logo.svg/512px-Samsonite_logo.svg.png",
  "banner":"https://images.unsplash.com/photo-1553062407-98eeb64c6a62?w=1400&q=80",
  "email":"sac@samsonite.com.br","telefone":"(11) 3504-8888","endereco":"São Paulo - SP"
}'
echo ""

# Quiksilver/Roxy — CNPJ Boardriders
curl -s -X POST "$API/lojas" -H "$H" -d '{
  "id":0,"nome":"Quiksilver","cnpj":"06057223000171",
  "descricao":"Surf, neve e outdoor. Roupas e equipamentos para quem vive a natureza.",
  "logo":"https://upload.wikimedia.org/wikipedia/commons/thumb/b/b5/Quiksilver_logo.svg/512px-Quiksilver_logo.svg.png",
  "banner":"https://images.unsplash.com/photo-1502680390548-bdbac40b3e1a?w=1400&q=80",
  "email":"atendimento@quiksilver.com.br","telefone":"(11) 3047-4700","endereco":"São Paulo - SP"
}'
echo ""

# Trilhas & Rumos — CNPJ Centauro
curl -s -X POST "$API/lojas" -H "$H" -d '{
  "id":0,"nome":"Trilhas & Rumos","cnpj":"06312878000107",
  "descricao":"Especializada em trekking, montanhismo e viagens de aventura. Equipamentos técnicos.",
  "logo":"https://images.unsplash.com/photo-1551632811-561732d1e306?w=200&q=80",
  "banner":"https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?w=1400&q=80",
  "email":"contato@trilhaserumos.com.br","telefone":"(11) 3062-8100","endereco":"São Paulo - SP"
}'
echo ""

echo "=== Lojas criadas. Buscando IDs... ==="
curl -s "$API/lojas" | python3 -c "import sys,json; [print(f\"  {l['id']}: {l['nome']}\") for l in json.loads(sys.stdin.read())]" 2>/dev/null || curl -s "$API/lojas"
echo ""
