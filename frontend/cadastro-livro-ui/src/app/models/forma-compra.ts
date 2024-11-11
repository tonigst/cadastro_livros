export class FormaCompra {
    constructor(codFC: number = 0, descricao: string = '') {
        this.codFC = codFC;
        this.descricao = descricao;
    }

    public codFC: number;
    public descricao: string;
}