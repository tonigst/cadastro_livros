import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BackendService {

  constructor(private httpClient: HttpClient) {    
  }

  ping(url: string, port: number) : Observable<any> {
    url = `/Main/pingtest/${encodeURIComponent(url)}/${port}`;
    let httpHeaders = new HttpHeaders()
    .set('accept', '*/*');
    return this.httpClient.get<string>(url, {headers: httpHeaders, observe: 'response'});
  }  
}
