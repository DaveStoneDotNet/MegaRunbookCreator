
import { Server } from './server.entity';

export class Environment {

    constructor(
        public Id:        number,
        public SortOrder: number,
        public Name:      string
    ) { }

}