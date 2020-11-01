import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.css']
})
export class LoaderComponent implements OnInit {
  loaderSize: string;

  constructor() {}

  ngOnInit(): void {
    let mappedSizes = new Map<SizeOptions, string>();
    mappedSizes.set('small', '1rem');
    mappedSizes.set('medium', '3rem');
    mappedSizes.set('large', '5rem');
    this.loaderSize = mappedSizes.get(this.size);
  }

  @Input() alignation: AlignOptions = 'center';
  @Input() color: string = '#5e6c95';
  @Input() size: SizeOptions = 'medium';

}

export type AlignOptions = 'center' | 'right' | 'left';
export type SizeOptions = 'small' | 'medium' | 'large';

