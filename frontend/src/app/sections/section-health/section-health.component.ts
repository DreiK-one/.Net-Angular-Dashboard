import { Observable } from 'rxjs';
import { ServerService } from './../../services/server.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Server } from 'src/app/shared/server';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-section-health',
  templateUrl: './section-health.component.html',
  styleUrls: ['./section-health.component.css']
})
export class SectionHealthComponent implements OnInit, OnDestroy {

  constructor(private _serverService: ServerService) { }

  servers?: Server[];
  timerSubsciption?: Subscription;

  ngOnInit(): void {
    this.refreshData();
    
  }

  ngOnDestroy(): void {
    if (this.timerSubsciption) {
      this.timerSubsciption.unsubscribe();
    }
  }

  refreshData() {
    this._serverService.getServers().subscribe(res => {
      this.servers = res;
    });

    this.subscribeToData();
  }

  subscribeToData() {
    // this.timerSubsciption = new Observable<number>().timer(5000).first()
    //   .subscribe(() => this.refreshData());
  }
}
