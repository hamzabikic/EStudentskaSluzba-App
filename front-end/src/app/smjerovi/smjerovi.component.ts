import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-smjerovi',
  templateUrl: './smjerovi.component.html',
  styleUrls: ['./smjerovi.component.css']
})
export class SmjeroviComponent implements OnInit {
  smjerovi:any;
  naziv ="";
  dialog_novi_smjer = false;
  constructor(private http:HttpClient) { }

  async ngOnInit(){
    await this.ucitajSmjerove();
  }
  async ucitajSmjerove(){
    this.smjerovi = await this.http.get(MojConfig.adresa_servera + "/Student/getSmjerovi").toPromise();
  }
  async obrisiSmjer(id:number) {
     let res = await this.http.delete
     (MojConfig.adresa_servera + "/Student/deleteSmjer?id="+id).toPromise();
     if(res) {
       alert("Uspjesno brisanje smjera!");
       await this.ucitajSmjerove();
       return;
     }
     alert("Neuspjesno brisanje smjera!");
  }
  async dodajSmjer(){
    let obj = {
      naziv: this.naziv
    };
    let res = await this.http.post(MojConfig.adresa_servera+"/Student/addSmjer",obj).toPromise();
    if(res) {
      alert("Uspjesno dodan smjer!");
      this.dialog_novi_smjer = !this.dialog_novi_smjer;
      await this.ucitajSmjerove();
      return;
    }
    alert("Neuspjesno dodavanje smjera!");
  }
}
