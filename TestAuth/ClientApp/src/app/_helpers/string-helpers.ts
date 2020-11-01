import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})

export class StringHelpers {

  isEmpty(str: string) : Boolean {
    return 0 === str.trim().length;
  }

}
