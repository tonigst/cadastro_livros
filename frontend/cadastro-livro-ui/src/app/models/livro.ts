import { Assunto } from "./assunto";
import { Autor } from "./autor";
import { Preco } from "./preco";

export class Livro {
    constructor(codL: number = 0, titulo: string = '', editora: string = '', edicao: number = 0, anoPublicacao: string = '',
        autores: Autor[] = [], assuntos: Assunto[] = [],precos: Preco[] = []
    ) {
        this.codL = codL;
        this.titulo = titulo;
        this.editora = editora;
        this.edicao = edicao;
        this.anoPublicacao = anoPublicacao;
        this.autores = autores;
        this.assuntos = assuntos;
        this.precos = precos;
    }

    public codL: number;
    public titulo: string;
    public editora: string;
    public edicao: number;
    public anoPublicacao: string;

    public autores: Autor[];
    public assuntos: Assunto[];
    public precos: Preco[];
}