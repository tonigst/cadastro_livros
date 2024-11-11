import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Livro } from '../models/livro';

@Injectable({
  providedIn: 'root'
})
export class LivroService {

  constructor(private httpClient: HttpClient) {    
  }

  get(codL: number) : Observable<any> {    
    let url = `${environment.serverUrl}/Livro/${codL}`;
    let httpHeaders = new HttpHeaders()

    .set('accept', '*/*');
    return this.httpClient.get<Livro>(url, 
      {headers: httpHeaders, observe: 'response'});
  }

  insert(livro: Livro) : Observable<any> {    
    let url = `${environment.serverUrl}/Livro`;
    let httpHeaders = new HttpHeaders()

    .set('accept', '*/*');
    return this.httpClient.post<Livro>(url, livro,
      {headers: httpHeaders, observe: 'response'});
  }

  update(livro: Livro) : Observable<any> {    
    let url = `${environment.serverUrl}/Livro`;
    let httpHeaders = new HttpHeaders()

    .set('accept', '*/*');
    return this.httpClient.put(url, livro,
      {headers: httpHeaders, observe: 'response'});
  }

  delete(codL: number) : Observable<any> {    
    let url = `${environment.serverUrl}/Livro/${codL}`;
    let httpHeaders = new HttpHeaders()

    .set('accept', '*/*');
    return this.httpClient.delete<Livro>(url, 
      {headers: httpHeaders, observe: 'response'});
  }
  
  getPage(page: number, pageSize: number) : Observable<any> {    
    let url = `${environment.serverUrl}/Livro/page/${page}/${pageSize}`;
    let httpHeaders = new HttpHeaders()

    .set('accept', '*/*');
    return this.httpClient.get<Livro[]>(url, 
      {headers: httpHeaders, observe: 'response'});
  }
}
