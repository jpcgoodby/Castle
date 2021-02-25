import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment'
import { Castle } from './castle';

@Injectable({
  providedIn: 'root'
})
export class AppService {

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  private fileHttpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'text/plain' })
  };

  constructor(private http: HttpClient) {}

  create(castle: Castle): Observable<any> {
    return this.http.post<any>(environment.bsseUrl + '/castle/data', castle, this.httpOptions)
      .pipe(
        catchError(this.handleError<any>('Add castle'))
      );
  }

  get(): Observable<Castle[]> {
    return this.http.get<Castle[]>(environment.bsseUrl + '/castle/data')
      .pipe(
        tap(choirs => console.log('Catles retrieved!')),
        catchError(this.handleError<any[]>('Get castles', []))
      );
  }

  upload(file: FormData): Observable<any> {
    return this.http.post(environment.bsseUrl + '/castle/file', file)
      .pipe(
        tap(_ => console.log(`File uploaded`)),
        catchError(this.handleError<any[]>('Add file'))
      );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error);
      // TODO: better job of transforming error for user consumption
      console.log(`${operation} failed: ${error.message}`);
      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

}