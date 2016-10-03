
import { Environment }     from './environment.entity';
import { EnvironmentLink } from './environment-link.entity';

export class Server {

    constructor(
        public Id:             number,
        public Name:           string,
        public EnvrironmentId: number,

        public Environment:    Environment 
    ) { }

}