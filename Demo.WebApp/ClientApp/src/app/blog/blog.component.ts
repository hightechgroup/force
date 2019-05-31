import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'blog',
  templateUrl: './blog.component.html'
})
export class BlogComponent {
  public data: any;

  constructor(http: HttpClient) {
    http.get<any>('/api/Post').subscribe(result => {
      this.data = result;
    }, error => console.error(error));
  }
}
