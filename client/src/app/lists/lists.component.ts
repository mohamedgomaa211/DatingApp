import { Component } from '@angular/core';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent {

}

interface car{
  color:string,
  model:string,
  topSpeed?:number,
}

const car2:car={
color:"123",
model:"bmw"
}
let date:number=123;
date=123;