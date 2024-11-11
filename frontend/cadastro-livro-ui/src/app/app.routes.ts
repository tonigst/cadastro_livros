import { Routes } from '@angular/router';
import { LivrosComponent } from './components/livros/livros.component';
import { AutoresComponent } from './components/autores/autores.component';
import { AssuntosComponent } from './components/assuntos/assuntos.component';
import { FormasCompraComponent } from './components/formas-compra/formas-compra.component';

export const routes: Routes = [
    { path: '', redirectTo: '/livros', pathMatch: 'full' },
    { path: 'livros', component: LivrosComponent },
    { path: 'autores', component: AutoresComponent },
    { path: 'assuntos', component: AssuntosComponent },
    { path: 'formascompra', component: FormasCompraComponent },
];
