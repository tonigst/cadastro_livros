import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { LivrosComponent } from './components/livros/livros.component';
import { AutoresComponent } from './components/autores/autores.component';
import { AssuntosComponent } from './components/assuntos/assuntos.component';
import { FormasCompraComponent } from './components/formas-compra/formas-compra.component';
import { SpinnerComponent } from './components/spinner/spinner.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    HeaderComponent, 
    LivrosComponent,
    AutoresComponent,
    AssuntosComponent,
    FormasCompraComponent,
    SpinnerComponent
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title = 'cadastro-livro-ui';
}
