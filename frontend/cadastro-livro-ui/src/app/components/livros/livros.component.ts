import { Component } from '@angular/core';
import { LivroService } from '../../services/livro.service';
import { Livro } from '../../models/livro';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-livros',
  standalone: true,
  imports: [
    CommonModule,
  ],
  templateUrl: './livros.component.html',
  styleUrl: './livros.component.css'
})
export class LivrosComponent {
  public showSpinner: boolean = false;
  public livros: Livro[] = [];

  constructor(private livroService: LivroService) {
    this.load();
  }

  load() {
    this.showSpinner = true;
    this.livroService.getPage(1, 40).subscribe({
      error: (error => alert(`Erro ao carregar livros: ${JSON.stringify(error)}`)),
      next:  (response => this.loadLivrosResponse(response))
    });
  }

  loadLivrosResponse(response: any) {  
    let responseIsOk = this.checkIfHttpResponseIsOk(response, 'Erro ao carregar livros');    
    if (responseIsOk) {
      this.livros = response.body as Livro[];
    }
  }

  delete(livro: Livro) {
    if (confirm('Tem certeza que deseja deletar o livro?')){
      this.showSpinner = true;
      this.livroService.delete(livro.codL).subscribe({
        error: (error => alert(`Erro ao excluir livro: ${error}`)),
        next:  (response => this.deleteResponse(response))
      });
    }
  }

  deleteResponse(response: any): void {
    let responseIsOk = this.checkIfHttpResponseIsOk(response, 'Erro ao excluir livro');    
    if (responseIsOk) {
      alert('Livro excluído!')
    }
  }

  edit(livro: Livro) {
    // TODO: show dialog/html + this.livroService.update
  }


  checkIfHttpResponseIsOk(response: any, notOkMessage: string): boolean {
    this.showSpinner = false;
    if (!response || !response?.body || response?.status != 200) {
      alert(notOkMessage);
      return false;
    }
    return true;
  }

}
