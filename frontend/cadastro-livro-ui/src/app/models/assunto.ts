export class Assunto {
    constructor(codAs: number = 0, descricao: string = '') {
        this.codAs = codAs;
        this.descricao = descricao;
    }

    public codAs: number;
    public descricao: string;
}