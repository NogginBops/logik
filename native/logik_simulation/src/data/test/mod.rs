use crate::data::component::components::*;
use crate::{map, set};
use super::*;
use crate::data::component::statefuls::{SRFlipFlop, Constant};
use std::cell::Cell;

macro_rules! edge {
        ($subnet:expr, $component:expr, $port:expr, 0) => {
            Edge {
                subnet: $subnet,
                component: $component,
                port: $port,
                direction: EdgeDirection::ToComponent,
            }
        };
        ($subnet:expr, $component:expr, $port:expr, 1) => {
            Edge {
                subnet: $subnet,
                component: $component,
                port: $port,
                direction: EdgeDirection::Bidirectional,
            }
        };
        ($subnet:expr, $component:expr, $port:expr, 2) => {
            Edge {
                subnet: $subnet,
                component: $component,
                port: $port,
                direction: EdgeDirection::ToSubnet,
            }
        };
    }

macro_rules! subnet {
        ($state:expr) => {
            {
                let mut s = Subnet::new();
                s.update($state);
                s
            }
        };
    }

#[test]
fn test_adding_components() {
    let mut data = Data::new();
    
    data.add_subnet(0);
    
    assert!(data.add_component(Box::new(OutputGate {}), vec![Some(0)]).is_ok());
    
    data.add_subnet(1);
    data.add_subnet(5);
    
    assert!(data.add_component(Box::new(AND {}), vec![Some(1), Some(5), Some(0)]).is_ok());
    
    assert!(data.add_component(Box::new(OutputGate {}), vec![Some(0)]).is_ok());
    
    assert_eq!(data.component_edges, map!(
        1 => set!(edge!(0, 1, 0, 0)),
        2 => set!(edge!(0, 2, 2, 2), edge!(1, 2, 0, 0), edge!(5, 2, 1, 0)),
        3 => set!(edge!(0, 3, 0, 0))
    ));
    assert_eq!(data.subnet_edges, map!(
        0 => set!(edge!(0, 1, 0, 0), edge!(0, 2, 2, 2), edge!(0, 3, 0, 0)),
        1 => set!(edge!(1, 2, 0, 0)),
        5 => set!(edge!(5, 2, 1, 0))
    ));
    
    assert!(data.add_component(Box::new(AND {}), vec![]).is_err());
}

#[test]
fn test_removing_subnets() {
    let mut data = Data::new();
    
    data.add_subnet(0);
    data.add_subnet(1);
    
    assert_eq!(data.component_edges, map!());
    assert_eq!(data.subnet_edges, map!());
    
    assert!(data.add_component(Box::new(OutputGate {}), vec![Some(0)]).is_ok());
    
    assert_eq!(data.component_edges, map!(
        1 => set!(edge!(0, 1, 0, 0))
    ));
    assert_eq!(data.subnet_edges, map!(
        0 => set!(edge!(0, 1, 0, 0))
    ));
    
    assert!(data.remove_subnet(0));
    
    assert_eq!(data.component_edges, map!());
    assert_eq!(data.subnet_edges, map!());
    
    assert!(data.remove_subnet(1));
    
    assert_eq!(data.component_edges, map!());
    assert_eq!(data.subnet_edges, map!());
    
    assert!(!data.remove_subnet(0));
    assert!(!data.remove_subnet(3));
}

#[test]
fn test_simulation() {
    let mut data = Data::new();
    
    data.add_subnet(0);
    data.add_subnet(1);
    
    assert!(data.add_component(Box::new(NOT {}), vec![Some(0), Some(1)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(false) }), vec![Some(0)]).is_ok());
    
    assert_eq!(data.simulation.dirty_subnets, VecDeque::from(vec![]));
    assert_eq!(data.subnets, map!(
        0 => subnet!(SubnetState::Off),
        1 => subnet!(SubnetState::On)
    ));
}

#[test]
fn test_simulation_2() {
    let mut data = Data::new();
    
    data.add_subnet(1);
    data.add_subnet(2);
    data.add_subnet(5);
    data.add_subnet(7);
    
    assert!(data.add_component(Box::new(Constant { state: Cell::new(false) }), vec![Some(7)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(true) }), vec![Some(2)]).is_ok());
    assert!(data.add_component(Box::new(AND {}), vec![Some(1), Some(2), Some(5)]).is_ok());
    assert!(data.add_component(Box::new(NOT {}), vec![Some(7), Some(1)]).is_ok());
    
    assert_eq!(data.simulation.dirty_subnets, VecDeque::from(vec![]));
    assert_eq!(data.subnets, map!(
        1 => subnet!(SubnetState::On),
        2 => subnet!(SubnetState::On),
        5 => subnet!(SubnetState::On),
        7 => subnet!(SubnetState::Off)
    ));
}

#[test]
fn test_simulation_3() {
    let mut data = Data::new();
    
    data.add_subnet(1);
    data.add_subnet(2);
    data.add_subnet(3);
    data.add_subnet(4);
    data.add_subnet(5);
    data.add_subnet(6);
    
    assert!(data.add_component(Box::new(AND {}), vec![Some(3), Some(3), Some(4)]).is_ok());
    assert!(data.add_component(Box::new(AND {}), vec![Some(1), Some(2), Some(3)]).is_ok());
    assert!(data.add_component(Box::new(NOT {}), vec![Some(5), Some(1)]).is_ok());
    assert!(data.add_component(Box::new(NOT {}), vec![Some(6), Some(2)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(false) }), vec![Some(5)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(false) }), vec![Some(6)]).is_ok());
    
    assert_eq!(data.simulation.dirty_subnets, VecDeque::from(vec![]));
    assert_eq!(data.subnets, map!(
        1 => subnet!(SubnetState::On),
        2 => subnet!(SubnetState::On),
        3 => subnet!(SubnetState::On),
        4 => subnet!(SubnetState::On),
        5 => subnet!(SubnetState::Off),
        6 => subnet!(SubnetState::Off)
    ));
}

// probably easiest to implement with buttons and such
//#[test]
fn test_sr_latch() {
    let mut data = Data::new();
    
    data.add_subnet(0);
    data.add_subnet(1);
    data.add_subnet(2);
    data.add_subnet(3);
    data.add_subnet(4);
    data.add_subnet(5);
    
    assert!(data.add_component(Box::new( SRFlipFlop { state: Cell::new(false) }),
                       vec![Some(0), Some(1), Some(2), Some(3), Some(4), Some(5)]).is_ok());
    assert!(data.add_component(Box::new( Constant { state: Cell::new(true) }), vec![Some(0)]).is_ok());
    assert!(data.add_component(Box::new( Constant { state: Cell::new(false) }), vec![Some(2)]).is_ok());
    
    data.update_subnet(0, SubnetState::On);
    data.update_subnet(2, SubnetState::Off);
    
    data.advance_time();
    
    assert_eq!(data.subnets, map!(
        0 => subnet!(SubnetState::On),
        1 => subnet!(SubnetState::Floating),
        2 => subnet!(SubnetState::Off),
        3 => subnet!(SubnetState::Floating),
        4 => subnet!(SubnetState::Off),
        5 => subnet!(SubnetState::On)
    ));
    
    data.update_subnet(2, SubnetState::On);
    
    data.advance_time();
    
    assert_eq!(data.subnets, map!(
        0 => subnet!(SubnetState::On),
        1 => subnet!(SubnetState::Floating),
        2 => subnet!(SubnetState::On),
        3 => subnet!(SubnetState::Floating),
        4 => subnet!(SubnetState::On),
        5 => subnet!(SubnetState::Off)
    ));
    
    data.update_subnet(0, SubnetState::Off);
    data.update_subnet(1, SubnetState::On);
    data.update_subnet(2, SubnetState::Off);
    
    data.advance_time();
    
    assert_eq!(data.subnets, map!(
        0 => subnet!(SubnetState::Off),
        1 => subnet!(SubnetState::On),
        2 => subnet!(SubnetState::Off),
        3 => subnet!(SubnetState::Floating),
        4 => subnet!(SubnetState::On),
        5 => subnet!(SubnetState::Off)
    ));
    
    data.update_subnet(2, SubnetState::On);
    
    data.advance_time();
    
    assert_eq!(data.subnets, map!(
        0 => subnet!(SubnetState::Off),
        1 => subnet!(SubnetState::On),
        2 => subnet!(SubnetState::On),
        3 => subnet!(SubnetState::Floating),
        4 => subnet!(SubnetState::Off),
        5 => subnet!(SubnetState::On)
    ));
}

#[test]
fn test_error_driving() {
    let mut data = Data::new();
    
    data.add_subnet(0);
    data.add_subnet(1);
    data.add_subnet(2);
    
    assert!(data.add_component(Box::new(Constant { state: Cell::new(false) }), vec![Some(0)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(true) }), vec![Some(1)]).is_ok());
    assert!(data.add_component(Box::new(NOT {}), vec![Some(0), Some(2)]).is_ok());
    assert!(data.add_component(Box::new(NOT {}), vec![Some(1), Some(2)]).is_ok());
    
    assert_eq!(data.subnets, map!(
        0 => subnet!(SubnetState::Off),
        1 => subnet!(SubnetState::On),
        2 => subnet!(SubnetState::Error)
    ));
}

#[test]
fn test_linking() {
    let mut data = Data::new();
    
    data.add_subnet(0);
    data.add_subnet(1);
    data.add_subnet(2);
    
    assert!(data.add_component(Box::new(AND {}), vec![Some(0), Some(1), Some(2)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(true) }), vec![Some(0)]).is_ok());
    assert!(data.add_component(Box::new(Constant { state: Cell::new(true) }), vec![Some(1)]).is_ok());
    
    assert_eq!(data.simulation.dirty_subnets, VecDeque::from(vec![]));
    
    assert_eq!(data.subnets.get(&2).unwrap().val(), SubnetState::On);
}

#[test]
fn test_simulating_loop() {
    let mut data = Data::new();
    
    data.add_subnet(1);
    
    assert!(data.add_component(Box::new(Constant::new()), vec![Some(1)]).is_ok());
    assert!(data.add_component(Box::new(NOT {}), vec![Some(1), Some(1)]).is_ok());
    
    assert_eq!(data.simulation.dirty_subnets, VecDeque::from(vec![]));
    assert_eq!(data.subnets, map!(
        1 => subnet!(SubnetState::Error)
    ));
}

#[test]
fn test_relinking() {
    let mut data = Data::new();
    
    data.add_subnet(1);
    data.add_subnet(2);
    
    assert!(data.add_component(Box::new(Constant::new()), vec![None]).is_ok());
    
    data.link(1, 0, 1);
    
    assert_eq!(data.component_edges, map!(
        1 => set!(edge!(1, 1, 0, 2))
    ));
    assert_eq!(data.subnet_edges, map!(
        1 => set!(edge!(1, 1, 0, 2))
    ));
    
    data.link(1, 0, 2);
    
    assert_eq!(data.component_edges, map!(
        1 => set!(edge!(2, 1, 0, 2))
    ));
    assert_eq!(data.subnet_edges, map!(
        2 => set!(edge!(2, 1, 0, 2))
    ));
}

#[test]
fn test_unlinking_last() {
    let mut data = Data::new();
    
    data.add_subnet(1);
    data.add_component(Box::new(Constant::new()), vec![None]).unwrap();
    
    assert_eq!(data.subnets, map!(
        1 => subnet!(SubnetState::Floating)
    ));
    
    assert!(data.link(1, 0, 1));
    
    assert_eq!(data.subnets, map!(
        1 => subnet!(SubnetState::Off)
    ));
    
    assert!(data.unlink(1, 0, 1));
    
    assert_eq!(data.subnets, map!(
        1 => subnet!(SubnetState::Floating)
    ));
}