import { Component, OnInit } from '@angular/core';
import { DataService } from '../shared/data.service';
import { IPandemic } from '../shared/interfaces';

@Component({
  selector: 'app-korona',
  templateUrl: './korona.component.html',
  styleUrls: ['./korona.component.css']
})
export class KoronaComponent implements OnInit {
  public korona: IPandemic;

  constructor(private dataService: DataService) {

  }

  ngOnInit(): void {
    this.dataService.getPandemic()
        .subscribe((res: IPandemic) => {
          this.korona = res;
          console.log(this.korona);
        },
        (err: any) => console.log(err),
        () => console.log('Pandemic data retrieved'));
  }

}
