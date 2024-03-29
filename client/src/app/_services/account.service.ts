import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../_models/User';
import { BehaviorSubject} from 'rxjs';

import { map } from 'rxjs';
import { environment } from 'src/environments/environment.development';


@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  baseUrl=environment.apiUrl;

  constructor(private http:HttpClient) {

   }
   login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    )
  }
  register(model:any){
    return this.http.post<User>(this.baseUrl+'account/register',model).pipe(
    map(response=>{
      const user=response;
      if(user){
        this.setCurrentUser(user);
      }
    }
    )
    )
  }
  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }
    setCurrentUser(user: User) {
      localStorage.setItem('user', JSON.stringify(user));
      this.currentUserSource.next(user);
    }
}
