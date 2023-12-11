import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users:any;
/**
 *
 */

constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getUsers();
  }
  registerToggle(){
    this.registerMode=!this.registerMode;
  }
  getUsers() {
    this.http.get('http://localhost:5139/api/users').subscribe({

      next: response => this.users = response,
      error: error => console.log(error),
      complete: () => console.log('Requst has completed')
    })
  }
  cancelEventHander(evnt:boolean){
    this.registerMode=evnt;

  }

}
