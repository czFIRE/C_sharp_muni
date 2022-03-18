import sys
import pandas
import pm4py
import time
import os

from pm4py.algo.discovery.dfg import algorithm as dfg_discovery
from pm4py.visualization.dfg import visualizer as dfg_visualization

from pm4py.algo.discovery.heuristics.variants import plusplus as heuristics_miner
from pm4py.visualization.heuristics_net import visualizer as hn_visualizer


CASE = ""
ACTIVITY = ""

OUTPUT_FILE = ""
   
def import_csv(file_path):
    transformed_file_path = file_path.replace(".","-t.")   
    
    with open(transformed_file_path, 'w') as w:
        with open(file_path, 'r') as r:
            first_line = True;
            seconds = 0;

            for x in r.readlines():
                if (first_line):
                    first_line = False
                    w.write(x.strip() + ";timestamp\n")
                    continue
            
                line = x.strip()

                if (not line.endswith(';')):
                    line += ';'   

                w.write(line + time.strftime('%H:%M:%S', time.gmtime(seconds)) + "\n")
                seconds += 1
    
    csv = transformed_file_path, pandas.read_csv(transformed_file_path, sep = ';', index_col = False)
    
    return csv


def get_event_log(csv):
    event_log = pm4py.format_dataframe(csv, case_id = CASE, activity_key = ACTIVITY, timestamp_key= "timestamp")
    
    start_activities = pm4py.get_start_activities(event_log)
    
    end_activities = pm4py.get_end_activities(event_log)
    
    return event_log

def generate_heuristics(event_log):
    heu_net = heuristics_miner.apply_heu(event_log, parameters={
        heuristics_miner.Parameters.DEPENDENCY_THRESH: -1,
        heuristics_miner.Parameters.AND_MEASURE_THRESH: 0.65,
        heuristics_miner.Parameters.MIN_ACT_COUNT: 0,
        heuristics_miner.Parameters.MIN_DFG_OCCURRENCES: 0
        })
    
    gviz = hn_visualizer.apply(heu_net)

    hn_visualizer.view(gviz)

    hn_visualizer.save(gviz, OUTPUT_FILE + "-heuristic" + ".jpg")

    return gviz


def main():
    input_file = sys.argv[1]

    global CASE, ACTIVITY, OUTPUT_FILE
    OUTPUT_FILE =  sys.argv[2]
    
    CASE =  sys.argv[3]
    ACTIVITY =  sys.argv[4]

    transformed_file_path, csv = import_csv(input_file)

    event_log = get_event_log(csv)

    generate_heuristics(event_log)
    
    if os.path.exists(transformed_file_path):
        os.remove(transformed_file_path)
    else:
        print("The file does not exist")



if __name__ == "__main__":
    main()
