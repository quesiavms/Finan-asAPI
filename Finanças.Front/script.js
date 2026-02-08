const API = 'http://localhost:5151/api/v1';
const content = document.getElementById('content');
const modal = document.getElementById('modal');
const modalContent = document.getElementById('modalContent');

function formatMoney(valor) {
  return Number(valor).toLocaleString('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  });
}

/* ================= MODAL ================= */
function openModal(type) {
  modal.classList.add('active');

  if (type === 'categoria') {
    modalContent.innerHTML = `
      <h3>Nova Categoria</h3>
      <input id="categoriaNome" placeholder="Nome da categoria">
      <button onclick="addCategoria()">Salvar</button>
    `;
  }

  if (type === 'entrada') {
    modalContent.innerHTML = `
      <h3>Nova Entrada</h3>
      <input id="entradaNome" placeholder="Nome">
      <input id="entradaValor" type="number" placeholder="Valor">
      <input id="entradaDesc" placeholder="Descrição">
      <button onclick="addEntrada()">Salvar</button>
    `;
  }

  if (type === 'saida') {
    modalContent.innerHTML = `
      <h3>Nova Saída</h3>
      <input id="saidaDesc" placeholder="Descrição">
      <input id="saidaValor" type="number" placeholder="Valor total">
      <input id="saidaParcelas" type="number" placeholder="Parcelas">
      <button onclick="addSaida()">Salvar</button>
    `;
  }
}

modal.onclick = e => {
  if (e.target === modal) modal.classList.remove('active');
};

/* ================= TELAS ================= */
async function showScreen(type) {
  if (type === 'categorias') {
    const res = await fetch(`${API}/Categorias`);
    const data = await res.json();
    data.sort((a, b) => a.idCategoria - b.idCategoria);

    content.innerHTML = `
      <table>
        <tr>
          <th>ID</th>
          <th>Categoria</th>
          <th>Ativa</th>
          <th>Ações</th>
        </tr>
        ${data.map(c => `
          <tr>
            <td>${c.idCategoria}</td>
            <td>${c.categoria}</td>
            <td>${c.isActive ? 'Ativo' : 'Inativo'}</td>
            <td>
            <button class="btn-edit" onclick='openEditCategoria(${JSON.stringify(c)})'>Editar</button>
            <button class="btn-delete" onclick="deleteCategoria(${c.idCategoria})">Inativar</button>
            </td>
          </tr>
        `).join('')}
      </table>
    `;
  }

  if (type === 'entradas') {
    const res = await fetch(`${API}/Entradas`);
    const data = await res.json();
    data.sort((a, b) => a.idEntrada - b.idEntrada)

    content.innerHTML = `
      <table>
        <tr>
          <th>ID</th>
          <th>Nome</th>
          <th>Valor</th>
          <th>Descrição</th>
          <th>Data</th>
          <th>Ações</th>
        </tr>
        ${data.map(e => `
          <tr>
            <td>${e.idEntrada}</td>
            <td>${e.nome}</td>
            <td>${formatMoney(e.valor)}</td>
            <td>${e.descricao}</td>
            <td>${e.date}</td>
            <td>
            <button class="btn-edit" onclick='openEditEntrada(${JSON.stringify(e)})'>Editar</button>
            <button class="btn-delete" onclick="deleteEntrada(${e.idEntrada})">Excluir</button>
            </td>
          </tr>
        `).join('')}
      </table>
    `;
  }

    if (type === 'saidas') {
    const res = await fetch(`${API}/Saida`);
    const data = await res.json();
    data.sort((a, b) => a.idSaida - b.idSaida)

    content.innerHTML = `
        <table>
        <tr>
            <th>ID</th>
            <th>Descrição</th>
            <th>Valor</th>
            <th>Categoria</th>
            <th>Pagamento</th>
            <th>Parcelas</th>
            <th></th>
            <th>Ações</th>
        </tr>

        ${data.map(s => `
            <tr>
            <td>${s.idSaida}</td>
            <td>${s.descricao}</td>
            <td>${formatMoney(s.valorTotal)}</td>
            <td>${s.categoria}</td>
            <td>${s.tipoPagamento}</td>
            <td>${s.parcelas.length > 1 ? `${s.parcelas.length}x` : 'À vista'}</td>
            <td>
                <button class="btn-edit" onclick='openParcelas(${JSON.stringify(s.parcelas)})'> Ver parcelas </button>
            </td>
            <td>
                <button class="btn-edit" onclick='openEditSaida(${JSON.stringify(s)})'>Editar</button>
                <button class="btn-delete" onclick="deleteSaida(${s.idSaida})">Excluir</button>
            </td>
            </tr>
        `).join('')}
        </table>
    `;
    }
}

/* ================= ACTIONS ================= */
async function addCategoria() {
  await fetch(`${API}/Categorias`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ categoria: categoriaNome.value })
  });
  modal.classList.remove('active');
  showScreen('categorias');
}

async function addEntrada() {
  await fetch(`${API}/Entradas`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      nome: entradaNome.value,
      valor: entradaValor.value,
      descricao: entradaDesc.value
    })
  });
  modal.classList.remove('active');
  showScreen('entradas');
}

async function addSaida() {
  await fetch(`${API}/Saida`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      descricao: saidaDesc.value,
      valorTotal: saidaValor.value,
      totalParcelas: saidaParcelas.value,
      dataCompra: new Date().toISOString(),
      idCategoria: 1,
      idTipoPagamento: 1
    })
  });
  modal.classList.remove('active');
  showScreen('saidas');
}

async function deleteCategoria(id) {
  await fetch(`${API}/Categorias/${id}`, { method: 'DELETE' });
  showScreen('categorias');
}

async function deleteEntrada(id) {
  await fetch(`${API}/Entradas/${id}`, { method: 'DELETE' });
  showScreen('entradas');
}

async function deleteSaida(id) {
  await fetch(`${API}/Saida?idsaida=${id}`, { method: 'DELETE' });
  showScreen('saidas');
}

function openEditEntrada(e) {
  modal.classList.add('active');
  modalContent.innerHTML = `
    <h3>Editar Entrada</h3>
    <input id="editNome" value="${e.nome}">
    <input id="editValor" type="number" value="${e.valor}">
    <input id="editDesc" value="${e.descricao}">
    <button onclick="updateEntrada(${e.idEntrada})">Salvar</button>
  `;
}

async function openEditSaida(s) {
  modal.classList.add('active');

  const categorias = await getOptions('Categorias');

  modalContent.innerHTML = `
    <h3>Editar Saída</h3>

    <input id="editDesc" value="${s.descricao}">
    <input id="editValor" type="number" value="${s.valorTotal}">

    <select id="editCategoria">
      ${categorias.map(c =>
        `<option value="${c.idCategoria}" ${c.categoria === s.categoria ? 'selected' : ''}>
          ${c.categoria}
        </option>`
      ).join('')}
    </select>

    <button onclick="updateSaida(${s.idSaida})">Salvar</button>
  `;
}

function openEditCategoria(c) {
  modal.classList.add('active');
  modalContent.innerHTML = `
    <h3>Editar Categoria</h3>

    <div class="form-group">
      <label>Nome</label>
      <input id="editCategoriaNome" value="${c.categoria}">
    </div>

    <button onclick="updateCategoria(${c.idCategoria})">Salvar</button>
  `;
}

function openParcelas(parcelas) {
  modal.classList.add('active');

  parcelas.sort((a, b) => a.numeroParcela - b.numeroParcela);

  modalContent.innerHTML = `
    <table>
      <thead>
        <tr>
          <th>Parcela</th>
          <th>Valor</th>
          <th>Vencimento</th>
          <th>Status</th>
        </tr>
      </thead>
      <tbody>
        ${parcelas.map(p => `
          <tr>
            <td>Parcela ${p.numeroParcela}</td>
            <td>${formatMoney(p.valor)}</td>
            <td> ${p.vencimento ? new Date(p.vencimento).toLocaleDateString('pt-BR') : '-'}
            </td>
            <td>
              <select ${p.pago ? 'disabled' : ''}
                onchange="pagarParcela(${p.idParcela})" >
                <option value="pendente" ${!p.pago ? 'selected' : ''}> Pendente </option>
                <option value="pago" ${p.pago ? 'selected' : ''}> Pago </option>
              </select>
            </td>
          </tr>
        `).join('')}
      </tbody>
    </table>
  `;
}

async function updateEntrada(id) {
  await fetch(`${API}/Entradas/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      nome: editNome.value,
      valor: editValor.value,
      descricao: editDesc.value
    })
  });
  modal.classList.remove('active');
  showScreen('entradas');
}

async function updateSaida(id) {
  await fetch(`${API}/Saida/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      descricao: editDesc.value,
      valorTotal: editValor.value,
      idCategoria: editCategoria.value,
      idTipoPagamento: editPagamento.value
    })
  });
  modal.classList.remove('active');
  showScreen('saidas');
}

async function updateCategoria(id) {
  await fetch(`${API}/Categorias/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      categoria: editCategoriaNome.value,
      isActive: editCategoriaAtiva.value == true
    })
  });

  modal.classList.remove('active');
  showScreen('categorias');
}

async function getOptions(endpoint, labelProp, idProp) {
  const res = await fetch(`${API}/${endpoint}`);
  return res.json();
}

async function pagarParcela(idParcela) {
  await fetch(`${API}/Parcela/PagarParcelaEspecifica=${idParcela}`, {
    method: 'PATCH'
  });
}
