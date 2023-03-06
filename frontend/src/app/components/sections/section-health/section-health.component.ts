import { ServerMessage } from '../../../shared/server-message';
import { Observable, timer } from 'rxjs';
import { ServerService } from '../../../services/server.service';
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
    // this.timerSubsciption = Observable.timer(5000).first()
    //   .subscribe(() => this.refreshData());

    this.timerSubsciption = timer(2000).subscribe(() => this.refreshData());
    
  }

  sendMessage(message: ServerMessage) {
    this._serverService.handleServerMessage(message)
      .subscribe((res: any) => console.log('Message sent to server: ', res), (error: any) => console.log('Error: ', error));
  }
}
