import { Component, OnInit, Input, Inject } from '@angular/core';

@Component({
  selector: 'app-image-with-text',
  templateUrl: './image-with-text.component.html',
  styleUrls: ['./image-with-text.component.css']
})
export class ImageWithTextComponent implements OnInit {
  url: string;

  constructor(
    @Inject('BASE_URL') private baseUrl: string
  ) {
    this.url = baseUrl;
  }

  ngOnInit(): void {
  }

  @Input() text: string;
  @Input() image: string;

}
