import { Component } from '@angular/core';
import { Location } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})

export class HeaderComponent {
  
  constructor(private location: Location)
  {}

   public click(event: any) {
  //   let element : HTMLElement = event.srcElement;
  //   let otherElements = document.getElementById('navbar')?.getElementsByClassName("nav-link");

  //   if (otherElements) 
  //     for (let i=0; i<otherElements?.length; i++) 
  //       otherElements[i].classList.remove('active');

  //   element.classList.add('active');
   }
}
